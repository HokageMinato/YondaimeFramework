using System.Collections.Generic;
using UnityEngine;
using System;

namespace YondaimeFramework
{
    public abstract class CustomBehaviour : MonoBehaviour
    {
        #region PRIVATE_VARIABLES
        [HideInInspector][SerializeField] private BehaviourLibrary _myLibrary;
        [HideInInspector][SerializeField] private SceneLibrary _sceneLibrary;
        #endregion

        #region PUBLIC_VARIABLES
        public int GOInstanceId { get { return id._goInsId; } }

        public BehaviourLibrary MyLibrary
        {
            get
            {
                CheckForMyLibNull();
                return _myLibrary;
            }
        }

        public SceneLibrary MySceneLibrary
        {
            get {
                CheckForSystemLibNull();
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


        /// <summary>
        /// GetComponent<T> Performance Alternative
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromMyGameObject<T>() 
        {
            int instanceId = gameObject.GetInstanceID();
            return MySceneLibrary.GetBehaviourOfGameObject<T>(instanceId);
        }

        /// <summary>
        /// FindObjectOfType<T> Performance Alternative
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>

        public List<T> GetComponentsFromMyGameObject<T>() 
        {
            List<T> behaviours = new List<T>();
            int instanceId = gameObject.GetInstanceID();
            MySceneLibrary.GetBehavioursOfGameObject<T>(instanceId,behaviours);
            return behaviours;
        }

        /// <summary>
        /// FindObjectOfType<T> Performance Alternative
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromLibrary<T>()
        {
            return MySceneLibrary.GetBehaviourFromLibrary<T>();
        }

        /// <summary>
        /// FindObjectsOfType Performance Alternative 
        /// </summary>
        /// <typeparam name="T"> Class, Interface </typeparam>
        /// <returns>List<typeparamref name="T"/> of Requested BehaviourType </returns>
        public List<T> GetComponentsFromLibrary<T>()
        {
            List<T> behaviours = new List<T>();
            MySceneLibrary.GetBehavioursFromLibrary<T>(behaviours);
            return behaviours;
        }

        /// <summary>
        /// FindObectOfType<UnityObject> Alternative with Interface support
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromLibraryById<T>(ComponentId behaviourId) 
        {
            return MySceneLibrary.GetBehaviourFromLibraryById<T>(behaviourId.objBt);
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromOtherSceneLibrary<T>(string sceneId) 
        {
            return MySceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehaviourFromLibrary<T>();
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene by ID
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromOtherSceneLibraryById<T>(ComponentId behaviourId,string sceneId) 
        {
            return MySceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehaviourFromLibraryById<T>(behaviourId.objBt);
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene by ID
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public List<T> GetComponentsFromOtherSceneLibrary<T>(string sceneId)
        {
            List<T> behaviours = new List<T>();
             MySceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehavioursFromLibrary<T>(behaviours);
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

        public void SetComponentIdExplicity(ComponentId newId) 
        { 
            id = newId;
        }
        #endregion

        #region PRIVATE_METHODS

        void CheckForSystemLibNull() 
        {
            if (_sceneLibrary == null)
            {
                throw new Exception($"Scene library not assigned at ({name}) Make sure to scan behaviours from scene library in editor");
            }
        }

        private void CheckForMyLibNull()
        {
            if (_myLibrary == null)
            {
                throw new Exception($"My library not assigned at ({name}) Make sure to scan behaviours from scene library in editor");
            }
        }

        #endregion

        #region COROUTINES
        #endregion

    }
}
