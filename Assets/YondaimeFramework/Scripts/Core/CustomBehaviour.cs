using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    public abstract class CustomBehaviour : MonoBehaviour
    {
        #region PRIVATE_VARIABLES
        [SerializeField] private BehaviourLibrary _myLibrary;
        [SerializeField] private SceneLibrary _sceneLibrary;
        [SerializeField] public ComponentId _id;
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
                return _sceneLibrary;
            }
        }

        public string Id
        {
            get
            {
                return _id.id;
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
            _sceneLibrary = library;
        }

        public T GetComponentFromMyGameObject<T>() 
        {
            return _sceneLibrary.GetBehaviourOfGameObject<T>(gameObject);
        }

        public List<T> GetComponentsFromMyGameObject<T>() 
        {
            return _sceneLibrary.GetBehavioursOfGameObject<T>(gameObject);
        }

        public T GetComponentFromLibrary<T>()
        {
            return _sceneLibrary.GetBehaviourFromLibrary<T>();
        }
        public List<T> GetComponentsFromLibrary<T>()
        {
            return _sceneLibrary.GetBehavioursFromLibrary<T>();
        }

        public T GetComponentFromLibraryById<T>(string behaviourId)
        {
            return _sceneLibrary.GetBehaviourFromLibraryById<T>(behaviourId);
        }

        public T GetComponentFromOtherSceneLibrary<T>(string sceneId)
        {
            return _sceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehaviourFromLibrary<T>();
        }
        
        public T GetComponentFromOtherSceneLibraryById<T>(string behaviourId,string sceneId)
        {
            return _sceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehaviourFromLibraryById<T>(behaviourId);
        }

        public List<T> GetComponentsFromOtherSceneLibrary<T>(string sceneId)
        {
            return _sceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehavioursFromLibrary<T>();
        }


        public virtual void RefreshHierarchy()
        {
            MyLibrary.ScanBehaviours();
            MyLibrary.InitializeLibrary();
        }

       
        public void DestorySelf()
        {
            DestroyImmediate(gameObject);
        }
        #endregion

        #region COROUTINES
        #endregion

    }
}
