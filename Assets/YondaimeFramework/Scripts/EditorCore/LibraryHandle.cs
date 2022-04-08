using YondaimeFramework;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEditor;


namespace YondaimeFramework.EditorHandles
{
    public class LibraryHandle : MonoBehaviour
    {

        public void ScanBehaviours()
        {
            FindSceneLibrary().SetBehaviours(FindObjectsOfType<CustomBehaviour>(true)); 
        }

        private ILibrary FindSceneLibrary() 
        {
            MonoBehaviour[] behv = FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour b in behv) 
            {
                if (b is ILibrary)
                {
                    return (ILibrary)b;
                }
            }

            return null;
        }

        
    }
}
