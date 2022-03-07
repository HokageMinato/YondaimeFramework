using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace YondaimeFramework
{
    public sealed class RootLibrary : CustomBehaviour
    {

        #region PRIVATE_VARS
        private Dictionary<string, SceneLibraryOld> _sceneLibLookUp = new Dictionary<string, SceneLibraryOld>();
        public static RootLibrary Instance;
        #endregion

        #region UNITY_CALLBACKS
        private void Awake()
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
                RaiseMultipleLibException();
                DestorySelf();
            }
        }
        #endregion

        #region PUBLIC_METHODS


        public SceneLibraryOld GetSceneLibraryById(string systemId)
        {
            return _sceneLibLookUp[systemId];
        }


        public void AddSceneLibrary(SceneLibraryOld newSceneLibrary) 
        {
            _sceneLibLookUp.Add(newSceneLibrary.SystemId, newSceneLibrary);
        }

        public void RemoveFromLibrary(string sceneId) {

            _sceneLibLookUp.Remove(sceneId);
        }
        #endregion


        #region PRIVATE_METHODS
        private void RaiseMultipleLibException() 
        {
            throw new Exception("A Root library already exists, There must be only one Root Library througout unity project");
        }
        #endregion
    }
}
