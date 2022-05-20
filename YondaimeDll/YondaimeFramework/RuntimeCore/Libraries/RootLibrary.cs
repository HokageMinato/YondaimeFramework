using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace YondaimeFramework
{
    public sealed class RootLibrary : MonoBehaviour
    {

        #region PRIVATE_VARS
        private Dictionary<string, ILibrary> _libLookUp = new Dictionary<string, ILibrary>();
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
            }
        }
        #endregion

        #region PUBLIC_METHODS


        public ILibrary GetSceneLibraryById(string systemId)
        {
            return _libLookUp[systemId];
        }


        public void AddSceneLibrary(ILibrary newSceneLibrary) 
        {
            _libLookUp.Add(newSceneLibrary.SceneId.id, newSceneLibrary);
        }

        public void RemoveFromLibrary(SceneId sceneId) {

            _libLookUp.Remove(sceneId.id);
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
