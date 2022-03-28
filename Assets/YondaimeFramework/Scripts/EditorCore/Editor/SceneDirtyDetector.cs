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
            HookOnSceneDirtied();
            HookOnUndo();
        }

        private static void HookOnSceneDirtied()
        {
            EditorSceneManager.sceneDirtied += OnSceneDirtied;
        }

        private static void HookOnUndo() 
        {
            Undo.undoRedoPerformed += OnUndoFired;
        }

        static void OnSceneDirtied(Scene scene)
        {
            InvokeBehavioursScan();
        }

        static void OnUndoFired()
        {
            InvokeBehavioursScan();
        }

        private static void InvokeBehavioursScan()
        {
            LibraryHandle handle = FindHandleFromScene();

            if (handle == null) 
                MissingLibraryException();
            
            handle.ScanBehaviours();

           
        }


        static LibraryHandle FindHandleFromScene() 
        {
            return GameObject.FindObjectOfType<LibraryHandle>();
        }


        static void MissingLibraryException()
        {
            throw new System.Exception("Please create a library or if already present please attach a library handle or framework functionalities wont work properly");
        }

    }

}