using System;
using System.Collections.Generic;
using UnityEngine;

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

        public SceneLibrary GetSceneLibraryFromRootLibraryById(string systemId)
        {
            return _rootLibrary.GetSceneLibraryById(systemId);
        }


        public override void PreRedundantCheck()
        {
            SetSystemLibrary();
        }

      
        public override void ScanBehaviours()
        {
            base.ScanBehaviours();
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
       
        private void RegisterSelfInRootLibrary() 
        {

            RootLibraryNonExistanceCheck();
            _rootLibrary = RootLibrary.Instance;
            _rootLibrary.AddSceneLibrary(this);
        }
        
        private void DeregisterSelfFromRootLibrary() 
        {
            _rootLibrary.RemoveFromLibrary(SystemId);
        }

        private void RootLibraryNonExistanceCheck() 
        {
            if (RootLibrary.Instance == null)
                throw new Exception("RootLibrary instance not found, either create one or check script execution order for Root Library to execute before scene library");
        }
        #endregion
    }
}
