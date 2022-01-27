using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;

public class Car : CustomBehaviour
{

    private void Start()
    {
        // Debug.Log($"{GetComponentFromLibraryById<Car>("Buggati").gameObject.name} -- {gameObject.name}" );
        // Debug.Log($"{GetComponentFromLibraryById<SportsCar>("Buggati").gameObject.name} -- {gameObject.name}" );
        
      //  Debug.Log($"{GetComponentFromOtherSceneLibrary<SportsCar>(SceneIDs.Scene1).gameObject.name} -- {gameObject.name}" );
       // Debug.Log($"{GetComponentFromOtherSceneLibraryById<Car>("Ferrari",SceneIDs.Scene1).gameObject.name} -- {gameObject.name}" );

    }


    private void Update()
    {
        
    }
}
