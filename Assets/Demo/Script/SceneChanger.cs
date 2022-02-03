using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(0);
        }    
        
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            SceneManager.LoadScene(1);
        }    
    }

}