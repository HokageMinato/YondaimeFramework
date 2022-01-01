using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tag
{
    public abstract class CustomBehaviour : MonoBehaviour
    {
        #region PRIVATE_VARIABLES
        [SerializeField]private CustomBehaviourLibrary _myLibrary;
        [HideInInspector][SerializeField] public string _id;
        #endregion

        #region PUBLIC_VARIABLES
        public CustomBehaviourLibrary ParentLibrary
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
        public void SetLibrary(CustomBehaviourLibrary library) {
            _myLibrary = library;
        }

        public void SetCustomId(string customId) {
            _id = customId;
        }

        public List<T> GetComponentsFromLibrary<T>() 
        {
            return _myLibrary.GetBehavioursFromLibrary<T>();
        }

        public List<T> GetComponentsFromChildLibrary<T>() {
            return _myLibrary.GetBehavioursFromChildLibrary<T>();
        }

        public T GetComponentFromLibrary<T>() {
            return _myLibrary.GetBehaviourFromLibrary<T>();
        }
        
        public T GetComponentFromChildLibrary<T>() {
            return _myLibrary.GetBehaviourFromLibrary<T>();
        }
      
        public virtual void RefreshHierarchy() 
        {
            ParentLibrary.ScanTypes();
        }


        public void DestorySelf() {
            DestroyImmediate(gameObject);
        }
        #endregion

        #region COROUTINES
        #endregion

    }
}
