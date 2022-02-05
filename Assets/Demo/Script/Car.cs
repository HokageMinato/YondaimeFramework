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

    
    private void Start()
    {
        //Debug.Log($"{GetComponentFromLibraryById<Car>("Buggati").gameObject.name} -- {gameObject.name}" );
        //Debug.Log($"{GetComponentFromLibraryById<SportsCar>("Buggati").gameObject.name} -- {gameObject.name}" );

        //Debug.Log($"{GetComponentFromOtherSceneLibrary<SportsCar>(SceneIDs.Scene1).gameObject.name} -- {gameObject.name}" );
        // Debug.Log($"{GetComponentFromOtherSceneLibraryById<Car>("Ferrari",SceneIDs.Scene1).gameObject.name} -- {gameObject.name}" );

        //Debug.Log($"{GetComponentsFromMyGameObject<SportsCar>()?.Count} -- {gameObject.name}" );
        //Debug.Log($"{GetComponentFromMyGameObject<SportsCar>() == null} -- {gameObject.name}" );

        //Car c = GetComponentFromLibraryById<Car>("Buggati");
        //c?.SetObjectId("Lamborghini");

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
            GetComponentFromLibraryById<Car>(idToSearch);

        }
        st.Stop();
        res = st.ElapsedMilliseconds.ToString();
        

        st.Reset();
        st.Start();
        for (double i = 0; i < maxItr; i++)
        {
            GameObject.FindGameObjectsWithTag("Finish");
        }
        st.Stop();


        Debug.Log($"{res} <Custom Unity> {st.ElapsedMilliseconds}");


    }

    [ContextMenu("Print")]
    public void Check()
    {
        RefreshHierarchy();
    }
}

public class SceneIDs 
{
    public const string Scene1 = "Scene1";
}