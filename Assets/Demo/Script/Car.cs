using UnityEngine;
using YondaimeFramework;
using Debug = UnityEngine.Debug;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

public class Car : CustomBehaviour, ICar
{

    private CustomBehaviour[] _behaviours;
    public Car[] othercars;
    public RuntimeIdContainer carIds;
    public SportsCar sports;

    public int itr;
    public Car otherCar;

    private Action ocAction;

    [ContextMenu("Call")]
    public void InvocationTest() 
    {
        Stopwatch st = new Stopwatch();
        st.Start();
        for (int i = 0; i < itr; i++)
        {
            otherCar.SomeMethod();
        }
        st.Stop();
        Debug.Log(st.ElapsedMilliseconds);

        ocAction = otherCar.SomeMethod;


        st.Reset();
        for (int i = 0; i < itr; i++)
        {
            ocAction();
        }
        st.Stop();
        
        Debug.Log(st.ElapsedMilliseconds);

    
    }



    #region BEHAVIOUR_LIBRARY_TESTS

    [ContextMenu("Test Instantiate")]
    public void Test()
    {
        ml.LogBehvLookup();
        Debug.Log($"before {sports.GetInstanceID()}");
        sports = Instantiate(sports, carIds.GetIds()[0]);

        Debug.Log($"After {sports.GetInstanceID()} > {sports.id.objBt}");
        ml.LogBehvLookup();
    }



    [ContextMenu("Test Destroy")]
    public void Test2()
    {
        ml.LogBehvLookup();
        DestroyCustom(sports);
        ml.LogBehvLookup();
    }


    [ContextMenu("Test Find By Tag")]
    void TestFindByTag()
    {

        ml.LogIdLookuip();
        Debug.Log($"Class : {GetComponentFromLibraryById<SportsCar>(carIds.GetIds()[1]) == null} -- {gameObject.name}");
        Debug.Log($"Interface : {GetComponentFromLibraryById<ICar>(carIds.GetIds()[0]) == null} -- {gameObject.name}");
        
    }
    
    [ContextMenu("Test GetComponent")]
    void TestGetComponent()
    {
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

    #endregion



    public void SomeMethod() 
    { 
    }

}







public class SceneIDs 
{
    public const string Scene1 = "Scene1";
}