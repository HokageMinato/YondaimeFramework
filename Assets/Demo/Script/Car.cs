using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;



public class Car : CustomBehaviour,ICar
{

    public int maxItr;
    public Car[] othercars;
    public ComponentId idToSearch;
    private const string tid = "Finish";

    
    private void Start()
    {
        //Debug.Log("===============================");
        //Debug.Log($"{GetComponentFromLibrary<SportsCar>() == null} -- {gameObject.name}");
        //Debug.Log($"{GetComponentFromLibraryById<SportsCar>(idToSearch) == null} -- {gameObject.name}");
        //Debug.Log($"{GetComponentFromMyGameObject<SportsCar>() == null} -- {gameObject.name}");
        //Debug.Log($"{GetComponentsFromLibrary<Car>()?.Count} -- {gameObject.name}" );
        //Debug.Log($"{GetComponentsFromMyGameObject<SportsCar>()?.Count} -- {gameObject.name}");
        //Debug.Log("===============================");

        Debug.Log(GetComponentFromLibraryById<Car>(idToSearch)==null);
    }

    
    [ContextMenu("Test")]
    public void PrintId() 
    {
        string res;

        Stopwatch st = new Stopwatch();
        st.Start();
        for (double i = 0; i < maxItr; i++)
        {
            GetComponentsFromLibrary<SportsCar>();
        }
        st.Stop();
        res = st.ElapsedMilliseconds.ToString();
        

        st.Reset();
        st.Start();
        for (double i = 0; i < maxItr; i++)
        {
            FindObjectsOfType<SportsCar>();
        }
        st.Stop();


        Debug.Log($"{res} <Custom Unity> {st.ElapsedMilliseconds}");


    }

    [ContextMenu("Test2")]
    public void Check()
    {
       

    }
}

public class SceneIDs 
{
    public const string Scene1 = "Scene1";
}