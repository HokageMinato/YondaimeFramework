using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;

public class CarHandler : CustomBehaviour
{
	//FillReferences is called before Init is called everytime parent library is initialized
    public override void FillReferences()
    {
        Debug.Log($"Filling References {gameObject.name}");
    }
	
	//Init is called everytime parent library is initialized
    public override void Init()
    {
        Debug.Log($"Initing from {gameObject.name}");
    }
	
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}