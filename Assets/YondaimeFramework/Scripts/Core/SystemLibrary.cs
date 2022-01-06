using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace YondaimeFramework
{
    public sealed class SystemLibrary : BehaviourLibrary
    {
        #region PRIVATE_VARS
        [SerializeField] private SystemId _systemId;
        [SerializeField] private RootLibrary _rootLibrary;
        [SerializeField] private bool IsSelfInited
        {
            get {
               return _rootLibrary == null;
            }
        }

        #endregion

        #region UNITY_CALLBACKS
        private void Awake()
        {
            if (IsSelfInited)
            {
                FrameworkLogger.Log("Self initing" + gameObject.name);
                InitializeLibrary();
            }
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


        #region PUBLIC_FUNCTIONS

        public void SetRootLibrary(RootLibrary library)
        {
            _rootLibrary = library;
        }

        public SystemLibrary GetSystemBehaviourFromRootLibraryById(string systemId)
        {
            return _rootLibrary.GetSystemBehaviourById(systemId);
        }

        public List<SystemLibrary> GetSystemBehavioursFromRootLibraryById(string systemId)
        {
            return _rootLibrary.GetSystemBehavioursById(systemId);
        }

        #endregion


        #region UI_CALLBACKS       

        [ContextMenu("Scan")]
        public override void ScanBehaviours()
        {
            base.ScanBehaviours();
            SetPresentSceneDirty();

            
        }


        public override void PreRedundantCheck()
        {
            SetSystemLibrary();
        }

        void SetSystemLibrary()
        {
            for (int i = 0; i < _behaviours.Count; i++)
            {
                _behaviours[i].SetLibrary(this);
            }
        }
        void SetPresentSceneDirty()
        {
            #if UNITY_EDITOR
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            #endif
        }
        #endregion
    }
}
