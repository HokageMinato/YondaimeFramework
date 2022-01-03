using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    public abstract class CustomBehaviour : MonoBehaviour
    {
        #region PRIVATE_VARIABLES
        [SerializeField] private BehaviourLibrary _myLibrary;
        [SerializeField] private BehaviourLibrary _systemLibrary;
        [HideInInspector][SerializeField] public string _id;
        #endregion

        #region PUBLIC_VARIABLES
        public BehaviourLibrary ParentLibrary
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

        public void SetSystemLibrary(BehaviourLibrary library) {
            _systemLibrary = library;
        }

        public void SetCustomId(string customId) {
            _id = customId;
        }

        public List<T> GetComponentsFromLibrary<T>() 
        {
            return _systemLibrary.GetBehavioursFromLibrary<T>();
        }

        public T GetComponentFromLibrary<T>() {
            return _systemLibrary.GetBehaviourFromLibrary<T>();
        } 
        
        public T GetComponentFromMyLibrary<T>() {
            return _myLibrary.GetBehaviourFromLibrary<T>();
        }
        
        public virtual void RefreshHierarchy() 
        {
            ParentLibrary.ScanBehaviours();
        }


        public void DestorySelf() {
            DestroyImmediate(gameObject);
        }
        #endregion

        #region COROUTINES
        #endregion

    }
}
