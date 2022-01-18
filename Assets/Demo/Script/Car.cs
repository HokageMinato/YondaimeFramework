using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;

public class Car : CustomBehaviour
{

    public override void FillReferences()
    {
        Debug.Log($"Filling references {gameObject.name}");
    }

    public override void Init()
    {
        Debug.Log($"Initting {gameObject.name}");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && gameObject.tag == "Finish") {
            DestorySelf();
            RefreshHierarchy();
        }
    }
}
