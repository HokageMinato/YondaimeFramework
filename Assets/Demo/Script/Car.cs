using UnityEngine;
using YondaimeFramework;
using Debug = UnityEngine.Debug;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

[System.Serializable]
public class CarData 
{
    public float speed;
    public string name;
}


public class Car : CustomBehaviour, ICar
{
    const string key = "Test1";
    const string key2 = "Test2";
    const string key3 = "Test3";
    const string key4 = "Test3";

    public PersistantVariable<int> intValue = new PersistantVariable<int>(key, 34);
    public PersistantVariable<float> floatValue = new PersistantVariable<float>(key2);
    public PersistantVariable<string> stringValue = new PersistantVariable<string>(key3);
    public PersistantVariable<CarData> jsonValue = new PersistantVariable<CarData>(key4, new CarData() { name = "Some" });

    public SceneId sceneIdToGetFrom;
    public Car[] othercars;
    public RuntimeIdContainer carIds;
    public SportsCar sports;
    public SportsCar sportsRef;
    public int itr;
    public Car otherCar;
    public CarData carData;
    public ComponentId idToGet;
    private ICar ocAction;


    #region BEHAVIOUR_LIBRARY_TESTS

    [ContextMenu("Test Instantiate")]
    public void Test()
    {
        ml.LogLookup();
        SportsCar c = Instantiate(sports);
        ml.LogLookup();
    }

    [ContextMenu("Test AddRef")]
    public void TestAddRef()
    {
        ml.LogLookup();
        sportsRef = MonoInstantiate(sports);
        CacheReference(sportsRef, true);
        ml.LogLookup();
    }

    [ContextMenu("Test SetId")]
    public void TestSetId()
    {
        ml.LogIdLookup();
        sports.SetId(idToGet);
        ml.LogIdLookup();

    }


    [ContextMenu("Test Destroy")]
    public void Test2()
    {
        ml.LogLookup();
        Destroy(sports, true);
        ml.LogLookup();
    }

    [ContextMenu("Test RemRef")]
    public void TestRemRef()
    {
        ml.LogLookup();
        DeCacheReference(sportsRef, true);
        ml.LogLookup();
    }


    [ContextMenu("Test Find By Tag")]
    void TestFindByTag()
    {
        FindObjectFromOtherSceneLibrary<SportsCar>(sceneIdToGetFrom.id);
        FindObjectFromOtherSceneLibraryById<SportsCar>(idToGet, sceneIdToGetFrom.id);

        ml.LogIdLookup();
        Debug.Log($"Class : {FindObjectFromLibraryById<SportsCar>(idToGet) == null} -- {gameObject.name}");
        Debug.Log($"Interface : {FindObjectFromLibraryById<ICar>(idToGet) == null} -- {gameObject.name}");

    }

    [ContextMenu("Test GetComponent")]
    void TestGetComponent()
    {

        Debug.Log($"Class :  {GetComponentFromMyGameObject<SportsCar>() == null} -- {gameObject.name}");
        Debug.Log($"Interface : {GetComponentFromMyGameObject<ICar>() == null} -- {gameObject.name}");
    }

    [ContextMenu("Test GetComponents")]
    void TestGetComponents()
    {
        ml.LogLookup();
        Debug.Log($"Class : {GetComponentsFromMyGameObject<SportsCar>()?.Count} -- {gameObject.name}");
        Debug.Log($"Interface : {GetComponentsFromMyGameObject<ICar>()?.Count} -- {gameObject.name}");
    }

    [ContextMenu("Test Find Object")]
    void TestFind()
    {
        Debug.Log($"Class : {FindObjectFromLibrary<SportsCar>() == null} -- {gameObject.name}");
        Debug.Log($"Interface : {FindObjectFromLibrary<ICar>() == null} -- {gameObject.name}");
    }

    [ContextMenu("Test Find Objects")]
    void TestFindObjects()
    {
        Debug.Log($"Class : {FindObjectsFromLibrary<Car>()?.Count} -- {gameObject.name}");
        Debug.Log($"Interface : {FindObjectsFromLibrary<ICar>()?.Count} -- {gameObject.name}");
    }


    [ContextMenu("Test Pool")]
    public void TestPool()
    {
        ml.LogLookup();
        Pool(sports);
        // sports = null;
        ml.LogLookup();
    }


    [ContextMenu("Test Get Pooled")]
    public void TestGetPool()
    {
        ml.LogLookup();
        sports = GetPooled<SportsCar>();
        ml.LogLookup();
    }


    [ContextMenu("Test GetComponentFromOtherScene ")]
    void TestGetComponentFromOtherScene()
    {
        Debug.Log($"Class : {FindObjectFromOtherSceneLibrary<Car>(sceneIdToGetFrom.id)} -- {gameObject.name}");
        Debug.Log($"Interface : {FindObjectFromOtherSceneLibrary<ICar>(sceneIdToGetFrom.id)} -- {gameObject.name}");
    }

    [ContextMenu("Test GetComponentsFromOtherScene")]
    void TestGetComponentsFromOtherScene()
    {
        Debug.Log($"Class : {FindObjectsFromOtherSceneLibrary<Car>(sceneIdToGetFrom.id)} -- {gameObject.name}");
        Debug.Log($"Interface : {FindObjectsFromOtherSceneLibrary<ICar>(sceneIdToGetFrom.id)} -- {gameObject.name}");
    }

    [ContextMenu("Test GetComponentFromOtherSceneById ")]
    void TestGetComponentFromOtherSceneById()
    {
        ComponentId idToGet = null;
        Debug.Log($"Class : {FindObjectFromOtherSceneLibraryById<Car>(idToGet, sceneIdToGetFrom.id)} -- {gameObject.name}");
        Debug.Log($"Class : {FindObjectFromOtherSceneLibraryById<ICar>(idToGet, sceneIdToGetFrom.id)} -- {gameObject.name}");
    }

    [ContextMenu("Log Library")]
    void Log() 
    {
        ml.LogLookup();
        ml.LogIdLookup();
        ml.LogGOLookup();
    }

    #endregion





    [ContextMenu("ref test")]
    public void InvocationTest()
    {
        Debug.Log(FindObjectsFromLibrary<Car>().Count);
    }

    [ContextMenu("SaveTest")]
    public void SaveTest()
    {
        intValue.Value = 2;
        floatValue.Value = 2;
        stringValue.Value = "2";
        jsonValue.Value = carData;
    }

    [ContextMenu("LoadTest")]
    public void LoadTest()
    {
        Debug.Log($"{intValue.Value}");
        Debug.Log($"{floatValue.Value}");
        Debug.Log($"{stringValue.Value}");
        Debug.Log($"{jsonValue.Value.name}");
        CarData data = jsonValue.Value;
        data.name = "faffari";
        jsonValue.Value = data;
        Debug.Log($"{jsonValue.Value.name}");
    }

    [ContextMenu("Test GetPooled Pool")]
    public void InvocationTestPooled()
    {
        SportsCar[] cars = new SportsCar[itr];
        for (int i = 0; i < itr; i++)
        {
            cars[i] = Instantiate(sports);
        }




        string data = string.Empty;
        Stopwatch st = new Stopwatch();
        st.Start();
        for (int i = 0; i < itr; i++)
        {
            Pool(cars[i]);
        }

        st.Stop();
        data += $"Destory {st.ElapsedMilliseconds} - ";

        st.Reset();

        //    ml.LogLookup();
        //   ml.LogIdLookup();


        st.Start();


        for (int i = 0; i < itr; i++)
        {
            GetPooled<SportsCar>();
        }

        st.Stop();

        // ml.LogLookup();
        // ml.LogIdLookup();

        Debug.Log($" Instantiate {st.ElapsedMilliseconds} {data}");

    }

    [ContextMenu("Test GetComponents Performance")]
    public void InvocationTestGetComponents()
    {

        string data = string.Empty;
        Stopwatch st = new Stopwatch();
        st.Start();
        for (int i = 0; i < itr; i++)
        {
            GetComponent<SportsCar>();
        }

        st.Stop();
        data += $"Traditional {st.ElapsedMilliseconds} - ";

        st.Reset();

        //    ml.LogLookup();
        //   ml.LogIdLookup();


        st.Start();

        for (int i = 0; i < itr; i++)
        {
            GetComponentFromMyGameObject<SportsCar>();
        }

        st.Stop();

        data += st.ElapsedMilliseconds;
        // ml.LogLookup();
        // ml.LogIdLookup();

        Debug.Log($" Custom {data}");

    }


    public void OnCollisionEnter(Collision collision)
    {
     
    }

}







public class SceneIDs 
{
    public const string Scene1 = "Scene1";
}