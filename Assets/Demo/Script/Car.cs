using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;

public class Car : CustomBehaviour,ICar
{

    private void Start()
    {
        //Debug.Log($"{GetComponentFromLibraryById<Car>("Buggati").gameObject.name} -- {gameObject.name}" );
        //Debug.Log($"{GetComponentFromLibraryById<SportsCar>("Buggati").gameObject.name} -- {gameObject.name}" );
        
        Debug.Log($"{GetComponentFromOtherSceneLibrary<SportsCar>(SceneIDs.Scene1).gameObject.name} -- {gameObject.name}" );
        Debug.Log($"{GetComponentFromOtherSceneLibraryById<Car>("Ferrari",SceneIDs.Scene1).gameObject.name} -- {gameObject.name}" );
        
        //Debug.Log($"{GetComponentsFromMyGameObject<SportsCar>()?.Count} -- {gameObject.name}" );
        Car c = GetComponentFromLibraryById<Car>("Buggati");
        c?.SetObjectId("Lamborghini");
        
        

    }


    private void Update()
    {
        
    }
}

public class SceneIDs 
{
    public const string Scene1 = "Scene1";
}