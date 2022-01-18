using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    public abstract class CustomBehaviour : MonoBehaviour
    {
        #region PRIVATE_VARIABLES
        [SerializeField] private BehaviourLibrary _myLibrary;
        [SerializeField] private SceneLibrary _systemLibrary;
        [HideInInspector] [SerializeField] public string _id;
        #endregion

        #region PUBLIC_VARIABLES
        public BehaviourLibrary MyLibrary
        {
            get
            {
                return _myLibrary;
            }
        }

        public SceneLibrary MySystemLibrary
        {
            get {
                return _systemLibrary;
            }
        }

        public string Id
        {
            get
            {
                return _id;
            }
        }
        #endregion

        #region PUBLIC_METHODS
        public void SetLibrary(BehaviourLibrary library)
        {
            _myLibrary = library;
        }

        public void SetLibrary(SceneLibrary library)
        {
            _systemLibrary = library;
        }

        public void SetCustomId(string customId)
        {
            _id = customId;
        }


        public T GetComponentFromLibrary<T>()
        {
            return _systemLibrary.GetBehaviourFromLibrary<T>();
        }
        public List<T> GetComponentsFromLibrary<T>()
        {
            return _systemLibrary.GetBehavioursFromLibrary<T>();
        }

        public T FindComponentFromLibrary<T>(string systemId)
        {
            return _systemLibrary.GetSystemBehaviourFromRootLibraryById(systemId).GetBehaviourFromLibrary<T>();
        }
        public List<T> FindComponentsFromLibrary<T>(string systemId)
        {
            return _systemLibrary.GetSystemBehaviourFromRootLibraryById(systemId).GetBehavioursFromLibrary<T>();
        }


        public virtual void RefreshHierarchy()
        {
            MyLibrary.ScanBehaviours();
            MyLibrary.InitializeLibrary();
            MyLibrary.InvokeFillReferences();
            MyLibrary.InvokeInit();
        }

        public virtual void FillReferences() {}

        public virtual void Init() {}

        public void DestorySelf()
        {
            DestroyImmediate(gameObject);
        }
        #endregion

        #region COROUTINES
        #endregion

    }
}
