using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace YondaimeFramework.EditorHandles{

    [InitializeOnLoad]
    public class SceneDirtyDetector : CustomBehaviour
    {
        static float t;
        private const float triggerFrequency = 1 * 1000;

        static SceneDirtyDetector()
        {
            EditorApplication.update += () =>
            {
                ScanPerSecond(InvokeBehavioursScan);
            };
        }


        

        private static void ScanPerSecond(Action invokeAction)
        {
            t += Time.time;

            if (t >= triggerFrequency)
            {
                invokeAction();
                t = 0;
            }
        }

        private static void InvokeBehavioursScan()
        {
            FindHandleFromScene()?.ScanBehaviours();
        }


        static LibraryHandle FindHandleFromScene() 
        {
            return FindObjectOfType<LibraryHandle>();
        }

    }

}