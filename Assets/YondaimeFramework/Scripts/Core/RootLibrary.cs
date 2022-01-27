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
        //public List<SceneLibrary> _sceneLibs = new List<SceneLibrary>();
        public static RootLibrary Instance;
        #endregion

        #region UNITY_CALLBACKS
        private void OnEnable()
        {
            GenerateSingleton();
            InitializeFramework();
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
                DestorySelf();
            }
        }
        #endregion

        #region PUBLIC_METHODS

        public void InitializeFramework()
        {
          //  InitializeLookUp();
            LogSystemLibraries();

            //void InitializeLookUp()
            //{
            //    for (int i = 0; i < _sceneLibs.Count; i++)
            //    {
            //        SceneLibrary lib = _sceneLibs[i];
            //        _sceneLibLookUp.Add(lib.Id, lib);
                    
            //    }
            //}
            
            void LogSystemLibraries()
            {
                if (FrameworkConstants.IsDebug)
                    foreach (var item in _sceneLibLookUp)
                    {
                        FrameworkLogger.Log($"System Library Added with key {item.Key} count {item.Value}");
                    }
            }

        }


        public SceneLibrary GetSceneLibraryById(string systemId)
        {
            return _sceneLibLookUp[systemId];
        }


        public void AddSceneLibrary(SceneLibrary newSceneLibrary) 
        {
           // _sceneLibs.Add(newSceneLibrary);
            _sceneLibLookUp.Add(newSceneLibrary.SystemId, newSceneLibrary);
        }

        public void RemoveFromLibrary(string sceneId) {

            //for (int i = 0; i < _sceneLibs.Count;)
            //{
            //    if (_sceneLibs[i].SystemId == sceneId)
            //        _sceneLibs.RemoveAt(i);
            //    else
            //        i++;
            //}

            _sceneLibLookUp.Remove(sceneId);
        }
        #endregion
    }
}
