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
       
       
        #endregion

        #region PUBLIC_VARIABLES
        public int GOInstanceId { get { return id._goInsId; } }

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
           id._goInsId = gameObject.GetInstanceID();
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
            List<T> behaviours = new List<T>();
            int instanceId = gameObject.GetInstanceID();
            _sceneLibrary.GetBehavioursOfGameObject<T>(instanceId,behaviours);
            return behaviours;
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
            List<T> behaviours = new List<T>();
            _sceneLibrary.GetBehavioursFromLibrary<T>(behaviours);
            return behaviours;
        }

        public T GetComponentFromLibraryById<T>(ComponentId behaviourId) 
        {
            return _sceneLibrary.GetBehaviourFromLibraryById<T>(behaviourId.objBt);
        }

        public T GetComponentFromOtherSceneLibrary<T>(string sceneId) 
        {
            return _sceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehaviourFromLibrary<T>();
        }
        
        public T GetComponentFromOtherSceneLibraryById<T>(ComponentId behaviourId,string sceneId) 
        {
            return _sceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehaviourFromLibraryById<T>(behaviourId.objBt);
        }

        public List<T> GetComponentsFromOtherSceneLibrary<T>(string sceneId)
        {
            List<T> behaviours = new List<T>();
             _sceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehavioursFromLibrary<T>(behaviours);
            return behaviours;
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

        public static int GenerateHashCode(string val)
        {
            return val.GetHashCode() * 92821;  // PRIME = 92821 or another prime number.
        }
        #endregion

        #region COROUTINES
        #endregion

    }
}
