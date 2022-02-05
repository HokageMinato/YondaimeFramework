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
        [HideInInspector] [SerializeField] public string _objectId;

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

        public string ObjectId
        {
            get
            {
                return _objectId;
            }
        }
        #endregion

        #region PUBLIC_METHODS
        public void RefreshGOInstanceId() 
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
            Stopwatch st = new Stopwatch();
            st.Start();
            MyLibrary.ScanBehaviours();
            MyLibrary.InitializeLibrary();
            st.Stop();

            Debug.Log(st.ElapsedMilliseconds);
            
        }


        public void SetObjectId(string objectId) 
        {
            _objectId = objectId;
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
