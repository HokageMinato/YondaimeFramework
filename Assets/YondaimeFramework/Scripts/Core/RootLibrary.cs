using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace YondaimeFramework
{
    
    public sealed class RootLibrary : CustomBehaviour
    {

        #region PRIVATE_VARS
        private Dictionary<string, SceneLibrary> _sceneLibLookUp = new Dictionary<string, SceneLibrary>();
        public static RootLibrary Instance;
        #endregion

        #region UNITY_CALLBACKS
        private void OnEnable()
        {
            GenerateSingleton();
        }
        #endregion


        #region PRIVATE_METHODS
        private void GenerateSingleton() 
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else {
                RaiseException();
                DestorySelf();
            }
        }
        #endregion

        #region PUBLIC_METHODS


        public SceneLibrary GetSceneLibraryById(string systemId)
        {
            return _sceneLibLookUp[systemId];
        }


        public void AddSceneLibrary(SceneLibrary newSceneLibrary) 
        {
            _sceneLibLookUp.Add(newSceneLibrary.SystemId, newSceneLibrary);
        }

        public void RemoveFromLibrary(string sceneId) {

            _sceneLibLookUp.Remove(sceneId);
        }
        #endregion


        #region PRIVATE_METHODS
        private void RaiseException() 
        {
            throw new Exception("A Root library already exists, There must be only one Root Library througout unity project");
        }
        #endregion
    }
}
