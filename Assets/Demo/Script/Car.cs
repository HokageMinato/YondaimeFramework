using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;



public class Car : CustomBehaviour,ICar
{

    public Car[] othercars;
    public ComponentId idToSearch;
    private const string tid = "Finish";
    

    
    private void Start()
    {
        //Debug.Log("===============================");
        //Debug.Log($"{GetComponentFromLibrary<SportsCar>() == null} -- {gameObject.name}");

        Debug.Log($"{GetComponentFromLibraryById<SportsCar>(idToSearch)?.gameObject.name} -- {gameObject.name}");
        //Debug.Log($"{GetComponentFromMyGameObject<SportsCar>() == null} -- {gameObject.name}");
        //Debug.Log($"{GetComponentsFromLibrary<Car>()?.Count} -- {gameObject.name}" );
        //Debug.Log($"{GetComponentsFromMyGameObject<SportsCar>()?.Count} -- {gameObject.name}");
        //Debug.Log("===============================");

    }

    
  
}

public class SceneIDs 
{
    public const string Scene1 = "Scene1";
}