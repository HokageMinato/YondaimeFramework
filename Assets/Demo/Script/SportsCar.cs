using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;

public interface ICar 
{
}

public class SportsCar : Car,ICar
{

    public void Copy() 
    { 
        Instantiate(this);
    }

    // Start is called before the first frame update
    [ContextMenu("Get")]
    void St()
    {
                   
        Debug.Log(ml == null);
        
    }

    // Update is called once per frame
    

}