using System;
using System.Collections.Generic;
using UnityEngine;


namespace YondaimeFramework
{
    public sealed class SystemLibrary : BehaviourLibrary
    {
        #region PRIVATE_VARS
        [SerializeField] private SystemId _systemId;
        [SerializeField] private RootLibrary _rootLibrary;
        #endregion

        #region PUBLIC_VARS
        public string SystemId
        {
            get
            {
                return _systemId.id;
            }
        }
        
        private bool IsSelfInited
        {
            get
            {
                return _rootLibrary == null;
            }
        }

        #endregion

        #region UNITY_CALLBACKS
        private void Awake()
        {
            if (IsSelfInited)
            {
                FramworkLogger.Log("Self initing" + gameObject.name);
                
                for (int i = 0; i < _childLibs.Length; i++)
                {
                    _childLibs[i].InitializeLibrary();
                }

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

        #endregion
    }
}
