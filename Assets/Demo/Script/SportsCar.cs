using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;

public interface ICar 
{
}

public class SportsCar : Car,ICar
{


    // Start is called before the first frame update
    [ContextMenu("Get")]
    void St()
    {
                   
        Debug.Log(typeof(ICar));
        Debug.Log(typeof(Car));
        Debug.Log(typeof(SportsCar));
    }

    // Update is called once per frame
    

}