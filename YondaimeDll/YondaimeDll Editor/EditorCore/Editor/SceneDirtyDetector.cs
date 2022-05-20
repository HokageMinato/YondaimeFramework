using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace YondaimeFramework.EditorHandles{

    [InitializeOnLoad]
    public class SceneDirtyDetector : CustomBehaviour
    {
        static float t;
        private static LibraryHandle handle;
        static SceneDirtyDetector()
        {
            EditorApplication.update += InvokeBehavioursScan;

        }

        private static void InvokeBehavioursScan()
        {
            if (handle == null)
                handle = FindObjectOfType<LibraryHandle>();

            handle.ScanBehaviours();
        }


        

    }

}