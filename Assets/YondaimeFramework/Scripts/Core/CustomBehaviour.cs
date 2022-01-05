using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    public abstract class CustomBehaviour : MonoBehaviour
    {
        #region PRIVATE_VARIABLES
        [SerializeField] private BehaviourLibrary _myLibrary;
        [SerializeField] private SystemLibrary _systemLibrary;

        //NA for now
        //[SerializeField] private RootLibrary _rootLibrary;
        [HideInInspector][SerializeField] public string _id;
        #endregion

        #region PUBLIC_VARIABLES
        public BehaviourLibrary MyLibrary
        {
            get
            {
                return _myLibrary;
            }
        }
      
        public string Id
        {
            get {
                return _id;
            }
        }
        #endregion

        #region PUBLIC_METHODS
        public void SetLibrary(BehaviourLibrary library) {
            _myLibrary = library;
        }

        public void SetLibrary(SystemLibrary library) {
            _systemLibrary = library;
        }

        public void SetCustomId(string customId) {
            _id = customId;
        }

        public List<T> GetComponentsFromLibrary<T>() 
        {
            return _systemLibrary.GetBehavioursFromLibrary<T>();
        }

        public List<T> GetComponentsFromLibrary<T>(string systemId)
        {
            return _systemLibrary.GetSystemBehaviourFromRootLibraryById(systemId).GetBehavioursFromLibrary<T>();
        }


        public T GetComponentFromLibrary<T>() {
            return _systemLibrary.GetBehaviourFromLibrary<T>();
        }

        public T GetComponentFromLibrary<T>(string systemId) 
        {
           return _systemLibrary.GetSystemBehaviourFromRootLibraryById(systemId).GetBehaviourFromLibrary<T>();
        }

        //public SystemLibrary GetComponentBySystemId(string systemId) {
        //    return _systemLibrary.GetSystemBehaviourFromRootLibraryById(systemId);
        //}
        
        //public List<SystemLibrary> GetComponentsBySystemId(string systemId) {
        //    return _systemLibrary.GetSystemBehavioursFromRootLibraryById(systemId);
        //}
        
        
        
        public virtual void RefreshHierarchy() 
        {
            MyLibrary.ScanBehaviours();
        }


        public void DestorySelf() {
            DestroyImmediate(gameObject);
        }
        #endregion

        #region COROUTINES
        #endregion

    }
}
