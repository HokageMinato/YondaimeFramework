using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace YondaimeFramework
{
    public abstract class CustomBehaviour : MonoBehaviour
    {
        #region PRIVATE_VARIABLES
        [HideInInspector] [SerializeField] private BehaviourLibrary _myLibrary;
        [HideInInspector] [SerializeField] private SceneLibrary _sceneLibrary;
       
        private int _insId;
        #endregion

        #region PUBLIC_VARIABLES
        public int GOInstanceId { get { return _insId; } }
       
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


        public ComponentId id;
        #endregion

        #region PUBLIC_METHODS
        public void RefreshIds() 
        {
            _insId = gameObject.GetInstanceID();
        }

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
            int instanceId = gameObject.GetInstanceID();
            return _sceneLibrary.GetBehaviourOfGameObject<T>(instanceId);
        }

        public List<T> GetComponentsFromMyGameObject<T>() 
        {
            int instanceId = gameObject.GetInstanceID();
            return _sceneLibrary.GetBehavioursOfGameObject<T>(instanceId);
        }

        /// <summary>
        /// FindObjectOfType Performance Alternative
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromLibrary<T>()
        {
            return _sceneLibrary.GetBehaviourFromLibrary<T>();
        }

        /// <summary>
        /// FindObjectsOfType Performance Alternative to 
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>List<typeparamref name="T"/> of Requested BehaviourType </returns>
        public List<T> GetComponentsFromLibrary<T>()
        {
            return _sceneLibrary.GetBehavioursFromLibrary<T>();
        }

        public T GetComponentFromLibraryById<T>(ComponentId behaviourId) 
        {
            return _sceneLibrary.GetBehaviourFromLibraryById<T>(behaviourId.objBt);
        }

        public T GetComponentFromOtherSceneLibrary<T>(string sceneId) where T: CustomBehaviour
        {
            return _sceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehaviourFromLibrary<T>();
        }
        
        public T GetComponentFromOtherSceneLibraryById<T>(ComponentId behaviourId,string sceneId) where T: CustomBehaviour
        {
            return _sceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehaviourFromLibraryById<T>(behaviourId.objBt);
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
