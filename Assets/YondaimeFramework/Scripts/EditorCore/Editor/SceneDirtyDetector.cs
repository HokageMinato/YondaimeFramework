using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using YondaimeFramework;
using UnityEngine.SceneManagement;

namespace YondaimeFramework.EditorHandles{

    [InitializeOnLoad]
    public class SceneDirtyDetector : CustomBehaviour
    {
        const string INIT_STATE = "SceneDirtyRegistered";
        
        static SceneDirtyDetector()
        {
            if (!IsOrderSetForThisSession())
            {
                EditorSceneManager.sceneDirtied += OnSceneDirtied;
                MarkRegisteredTrue();
            }
        }

        static void OnSceneDirtied(Scene scene) 
        {
            LibraryHandle handle = FindHandleFromScene(scene);
            handle?.ScanBehaviours();
        }

        static bool IsOrderSetForThisSession()
        {
            return SessionState.GetBool(INIT_STATE, false);
        }

        static void MarkRegisteredTrue()
        {
            SessionState.SetBool(INIT_STATE, true);
        }

        static LibraryHandle FindHandleFromScene(Scene scene) 
        {
            
            GameObject[] objects = scene.GetRootGameObjects();
            for (int i = 0; i < objects.Length; i++)
            {
                LibraryHandle handle= objects[i].GetComponentInChildren<LibraryHandle>(true);
                if (handle)
                    return handle;
            }

            return null;

        }


    }

}