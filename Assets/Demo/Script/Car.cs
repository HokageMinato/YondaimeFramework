using UnityEngine;
using YondaimeFramework;
using Debug = UnityEngine.Debug;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

public class Car : CustomBehaviour, ICar
{


    public Car[] othercars;
    public RuntimeIdContainer carIds;
    public SportsCar sports;

    public int itr;
    public Car otherCar;

    //private Func<int> ocAction;
    private ICar ocAction;
    
  
    #region BEHAVIOUR_LIBRARY_TESTS

    [ContextMenu("Test Instantiate")]
    public void Test()
    {
        ml.LogLookup();
        sports = Instantiate(sports, carIds.GetIds()[0]);
        ml.LogLookup();
    }



    [ContextMenu("Test Destroy")]
    public void Test2()
    {
        ml.LogLookup();
        Destroy(sports);
        ml.LogLookup();
    }


    [ContextMenu("Test Find By Tag")]
    void TestFindByTag()
    {

        ml.LogIdLookup();
        Debug.Log($"Class : {GetComponentFromLibraryById<SportsCar>(carIds.GetIds()[1]) == null} -- {gameObject.name}");
        Debug.Log($"Interface : {GetComponentFromLibraryById<ICar>(carIds.GetIds()[0]) == null} -- {gameObject.name}");
        
    }
    
    [ContextMenu("Test GetComponent")]
    void TestGetComponent()
    {
        ml.LogLookup();
        Debug.Log($"Class : {GetComponentFromMyGameObject<SportsCar>() == null} -- {gameObject.name}");
        Debug.Log($"Interface : {GetComponentFromMyGameObject<ICar>() == null} -- {gameObject.name}");
    }

    [ContextMenu("Test GetComponents")]
    void TestGetComponents()
    {
        Debug.Log($"Class : {GetComponentsFromMyGameObject<SportsCar>()?.Count} -- {gameObject.name}");
        Debug.Log($"Interface : {GetComponentsFromMyGameObject<SportsCar>()?.Count} -- {gameObject.name}");
    }

    [ContextMenu("Test Find Object")]
    void TestFind()
    {
        Debug.Log($"Class : {GetComponentFromLibrary<SportsCar>() == null} -- {gameObject.name}");
        Debug.Log($"Interface : {GetComponentFromLibrary<ICar>() == null} -- {gameObject.name}");
    }

    [ContextMenu("Test Find Objects")]
    void TestFindObjects()
    {
        Debug.Log($"Class : {GetComponentsFromLibrary<Car>()?.Count} -- {gameObject.name}");
        Debug.Log($"Interface : {GetComponentsFromLibrary<ICar>()?.Count} -- {gameObject.name}");
    }


    [ContextMenu("Test Pool")]
    public void TestPool()
    {
        ml.LogLookup();
        Pool(sports);
        sports = null;
        ml.LogLookup();
    }


    [ContextMenu("Test Get Pooled")]
    public void TestGetPool()
    {
        ml.LogLookup();
        sports = GetPooled<SportsCar>();
        ml.LogLookup();
    }



    
    #endregion



    [ContextMenu("Call")]
    public void InvocationTest()
    {
        Stopwatch st = new Stopwatch();
        st.Start();

        for (int i = 0; i < itr; i++)
        {
        }

        st.Stop();
        Debug.Log(st.ElapsedMilliseconds);

        st.Reset();
        st.Start();
        

        for (int i = 0; i < itr; i++)
        {
        }
       
        st.Stop();

        Debug.Log(st.ElapsedMilliseconds);


    }



}







public class SceneIDs 
{
    public const string Scene1 = "Scene1";
}