using YondaimeFramework;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System;

namespace YondaimeFramework.EditorHandles
{
    [RequireComponent(typeof(SceneLibraryOld))]
    public class SceneLibraryHandle : MonoBehaviour
    {
        [SerializeField] SceneLibraryOld sceneLibrary;
        [SerializeField] SceneLibrary sceneLibraryNew;

        

        [ContextMenu("Scan")]
        public void ScanBehaviours() 
        {
            sceneLibrary = FindObjectOfType<SceneLibraryOld>();
            sceneLibrary.ScanBehaviours();
            
            //sceneLibraryNew.SetRootBehaviourLibrary(sceneLibraryNew.GetComponent<BehaviourLibrary>());
            SetSceneDirty();
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
