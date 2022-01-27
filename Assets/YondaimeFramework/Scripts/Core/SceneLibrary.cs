using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace YondaimeFramework
{
    public sealed class SceneLibrary : BehaviourLibrary
    {
        #region PRIVATE_VARS
        [SerializeField] private SceneId _systemId;
        [SerializeField] private RootLibrary _rootLibrary;
        #endregion

        #region UNITY_CALLBACKS
        private void OnEnable()
        {
            FrameworkLogger.Log($"Init Scenen library {_systemId.id}");
            InitializeLibrary();
            RegisterSelfInRootLibrary();
        }

        private void OnDestroy()
        {
            DeregisterSelfFromRootLibrary();
        }
        #endregion

        #region PUBLIC_VARS
        public string SystemId
        {
            get
            {
                return _systemId.id;
            }
        }
        #endregion


        #region PUBLIC_METHODS

        public void SetRootLibrary(RootLibrary library)
        {
            _rootLibrary = library;
        }

        public new T GetBehaviourFromLibrary<T>() 
        {
            return base.GetBehaviourFromLibrary<T>();
        }

        public new List<T> GetBehavioursFromLibrary<T>()
        {
            //to protect direct access to GetBehaviour via myLib reference in CustomBehaviour
            return base.GetBehavioursFromLibrary<T>();
        }

        public new T GetBehaviourFromLibraryById<T>(string behaviourId)
        {
            //to protect direct access to GetBehaviour via myLib reference in CustomBehaviour
            return base.GetBehaviourFromLibraryById<T>(behaviourId);
        }
       
        public SceneLibrary GetSceneLibraryFromRootLibraryById(string systemId)
        {
            return _rootLibrary.GetSceneLibraryById(systemId);
        }



        public override void PreRedundantCheck()
        {
            SetSystemLibrary();
        }

        [ContextMenu("Scan")]
        public override void ScanBehaviours()
        {
            base.ScanBehaviours();
            SetPresentSceneDirty();
        }

        #endregion


        #region PRIVATE_METHODS      
        private void SetSystemLibrary()
        {
            for (int i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].SetLibrary(this);
            }
        }
        private void SetPresentSceneDirty()
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            #endif
        }

        private void RegisterSelfInRootLibrary() 
        {
            _rootLibrary = RootLibrary.Instance;
            _rootLibrary.AddSceneLibrary(this);
        }
        
        private void DeregisterSelfFromRootLibrary() 
        {
            _rootLibrary.RemoveFromLibrary(SystemId);
        }


        #endregion
    }
}
