using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YondaimeFramework
{
    public sealed class SystemLibrary : BehaviourLibrary
    {
        #region PRIVATE_VARS
        [SerializeField] private string _systemId;
        [SerializeField] private RootLibrary _rootLibrary;
        #endregion

        #region PUBLIC_VARS
        public string SystemId
        {
            get 
            {
                return _systemId;
            }
        }
        #endregion


        #region PUBLIC_FUNCTIONS

        public void SetRootLibrary(RootLibrary library) {
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
