using YondaimeFramework;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System;

namespace YondaimeFramework.EditorHandles
{
    [RequireComponent(typeof(SceneLibrary))]
    public class SceneLibraryHandle : MonoBehaviour
    {
        [SerializeField] SceneLibrary sceneLibrary;

        

        [ContextMenu("Scan")]
        public void ScanBehaviours()
        {
            FindSceneLibrary();
            SetSceneLibrary();
            SetSceneDirty();
        }

        private void FindSceneLibrary() 
        { 
            sceneLibrary = FindObjectOfType<SceneLibrary>();
        }

        private void SetSceneLibrary()
        {
            CustomBehaviour[] bhvs =FindObjectsOfType<CustomBehaviour>();
            sceneLibrary.SetBehaviours(bhvs);
            foreach (var item in bhvs)
            {
                item.SetLibrary(sceneLibrary);
            }
        }

        public void SetSceneDirty()
        {
            if (!Application.isPlaying)
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }


        #region SCENE_LIB_EDITOR_COUNTERPART

        

      

        #endregion
    }
}
