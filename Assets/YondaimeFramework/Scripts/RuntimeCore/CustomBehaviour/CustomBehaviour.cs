using System.Collections.Generic;
using UnityEngine;
using System;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace YondaimeFramework
{

    public abstract class CustomBehaviour : MonoBehaviour
    {
        #region LIBRARIES
        //[SerializeField] private SceneLibrary _mySceneLibrary;
        private ILibrary _myLibrary;
        #endregion

        #region IDS
        public ComponentId id;
        [HideInInspector] public int poolState;
        #endregion

        #region LIBRARY_HANDLES
        public void RefreshIds()
        {
            id._goInsId = gameObject.GetInstanceID();
        }

        public void SetLibrary(ILibrary library)
        {
            //_mySceneLibrary = library;
            _myLibrary = library;
        }
        #endregion

        #region POOL_CALLBACKS
        public virtual void OnPooled() 
        {
            gameObject.SetActive(false);
        }
        #endregion

        #region COMPONENT_GETTERS
        /// <summary>
        /// GetComponent<T> Performance Alternative
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromMyGameObject<T>()
        {
            CheckForSystemLibNull();
            int instanceId = id._goInsId;
            return _myLibrary.GetBehaviourOfGameObject<T>(instanceId);
        }

        /// <summary>
        /// FindObjectOfType<T> Performance Alternative
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>

        public IReadOnlyList<T> GetComponentsFromMyGameObject<T>()
        {
            CheckForSystemLibNull();
            int instanceId = id._goInsId;
            return _myLibrary.GetBehavioursOfGameObject<T>(instanceId);
        }

        /// <summary>
        /// FindObjectOfType<T> Performance Alternative
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromLibrary<T>()
        {
            CheckForSystemLibNull();
            return _myLibrary.GetBehaviourFromLibrary<T>();
        }

        /// <summary>
        /// FindObjectsOfType Performance Alternative 
        /// </summary>
        /// <typeparam name="T"> Class, Interface </typeparam>
        /// <returns>List<typeparamref name="T"/> of Requested BehaviourType </returns>
        public IReadOnlyList<T> GetComponentsFromLibrary<T>()
        {
            CheckForSystemLibNull();
            return _myLibrary.GetBehavioursFromLibrary<T>();
        }

        /// <summary>
        /// FindObectOfType<UnityObject> Alternative with Interface support
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromLibraryById<T>(ComponentId behaviourId)
        {
            CheckForSystemLibNull();
            return _myLibrary.GetBehaviourFromLibraryById<T>(behaviourId.objBt);
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromOtherSceneLibrary<T>(string sceneId)
        {
            CheckForSystemLibNull();
            return _myLibrary.GetComponentFromOtherSceneLibrary<T>(sceneId);
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene by ID
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromOtherSceneLibraryById<T>(ComponentId behaviourId, string sceneId)
        {
            CheckForSystemLibNull();
            return _myLibrary.GetComponentFromOtherSceneLibraryById<T>(behaviourId, sceneId);
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene by ID
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public IReadOnlyList<T> GetComponentsFromOtherSceneLibrary<T>(string sceneId)
        {
            CheckForSystemLibNull();
            return _myLibrary.GetComponentsFromOtherSceneLibrary<T>(sceneId);
        }
        #endregion
       
        #region INSTANTIATORS_DESTRUCTORS
        private void _Instantiate<T>(T newObject) where T : CustomBehaviour
        {
            CheckForSystemLibNull();
            _myLibrary.AddBehaviour(newObject);
        }

        public void SetId(ComponentId id)
        {
            CheckForSystemLibNull();
            _myLibrary.SetComponentId(this, id);

        }
        #endregion

        #region STATIC_CONSTRUCT_DESTRUCT_HANDLES
        public void Destroy<T>(T original,bool destoryGameObject=false) where T : CustomBehaviour 
        {
            CheckForNullObjDestory(original);

            GameObject go = original.gameObject;
            ComponentId id = original.id;
            Type t = original.GetType();
            DestroyImmediate(original);

            Debug.Log($"GONE {original == null}");
            _myLibrary.CleanNullReferencesFor(id,t);


            if (destoryGameObject)
                DestroyImmediate(go);
        }

        public T GetPooled<T>() 
        {
            CheckForSystemLibNull();
            return  _myLibrary.GetPooled<T>();
        }

        public void Pool(CustomBehaviour behaviour) 
        {
            CheckForSystemLibNull();
            _myLibrary.Pool(behaviour);
        }

        public new T Instantiate<T>(T original) where T : CustomBehaviour
        {
            T newBehaviour = MonoInstantiate(original);
            _Instantiate(newBehaviour);
            return newBehaviour;
        }

        public T Instantiate<T>(T original,ComponentId id) where T : CustomBehaviour
        {
            T newBehaviour = MonoInstantiate(original);
             original.id = id;
            _Instantiate(newBehaviour);
            return newBehaviour;
        }

        public new T Instantiate<T>(T original, Transform parent) where T : CustomBehaviour
        {
            T newBehaviour = MonoInstantiate(original, parent);
            _Instantiate(newBehaviour);
            return newBehaviour;
        }

        public new T Instantiate<T>(T original, Transform parent, bool instantiateInWorldSpace) where T : CustomBehaviour
        {
            T newBehaviour = MonoInstantiate(original, parent,instantiateInWorldSpace);
            _Instantiate(newBehaviour);
            return newBehaviour;
        }

        public new T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : CustomBehaviour
        {
            T newBehaviour = MonoInstantiate(original, position, rotation);
            _Instantiate(newBehaviour);
            return newBehaviour;
        }
        
        public new T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : CustomBehaviour
        {
            T newBehaviour = MonoInstantiate(original, position, rotation,parent);
            _Instantiate(newBehaviour);
            return newBehaviour;
        }
        #endregion

        #region MONO_WRAPPERS

        public static T MonoInstantiate<T>(T original) where T : UnityEngine.Object
        {
            return UnityEngine.Object.Instantiate(original);
        }

        public static T MonoInstantiate<T>(T original, Transform parent) where T : UnityEngine.Object
        {
           return UnityEngine.Object.Instantiate(original, parent);
        }

        public static T MonoInstantiate<T>(T original, Transform parent, bool instantiateInWorldSpace) where T : UnityEngine.Object
        {
            return UnityEngine.Object.Instantiate(original, parent,instantiateInWorldSpace);
        }

        public static T MonoInstantiate<T>(T original, Vector3 position, Quaternion rotation) where T : UnityEngine.Object
        {
            return UnityEngine.Object.Instantiate(original, position, rotation);
        }
        
        public static T MonoInstantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : UnityEngine.Object
        {
            return UnityEngine.Object.Instantiate(original, position, rotation,parent);
        }
        #endregion

        #region EXCEPTIONS
        void CheckForSystemLibNull()
        {
            if (_myLibrary == null)
            {
                throw new Exception($"Scene library not assigned at ({name}) Make sure to scan behaviours from scene library in editor or isntantiate from CustomBehaviour");
            }
        }

        void CheckForNullObjDestory<T>(T obj) 
        {
            if (obj == null)
                throw new Exception("Object you want to destory is null");
        }

        #endregion


    #if UNITY_EDITOR
        #region EDITOR

        public ILibrary ml => _myLibrary;

        public static int GenerateHashCode(string val)
        {
            return val.GetHashCode() * 92821;  // PRIME = 92821 or another prime number.
        }
        #endregion


    }
    #endif
}
