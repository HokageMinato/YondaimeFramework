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
        private static ILibrary _myLibrary;
        #endregion

        #region IDS
        public ComponentId id;
        [HideInInspector] public int poolState;
        #endregion

        #region LIBRARY_HANDLES
        public void RefreshIds()
        {
            if(id == null)
                id= new ComponentId();

            id._goInsId = gameObject.GetInstanceID();
        }

        public void SetLibrary(ILibrary library)
        {
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


        public T GetComponentFromOtherGameObject<T>(GameObject otherGameObject) 
        {
            CheckForSystemLibNull();
            int instanceId = otherGameObject.GetInstanceID();
            return _myLibrary.GetBehaviourOfGameObjectSafe<T>(instanceId);  
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
        public T FindObjectFromLibrary<T>()
        {
            CheckForSystemLibNull();
            return _myLibrary.GetBehaviourFromLibrary<T>();
        }

        /// <summary>
        /// FindObjectsOfType Performance Alternative 
        /// </summary>
        /// <typeparam name="T"> Class, Interface </typeparam>
        /// <returns>List<typeparamref name="T"/> of Requested BehaviourType </returns>
        public IReadOnlyList<T> FindObjectsFromLibrary<T>()
        {
            CheckForSystemLibNull();
            return _myLibrary.GetBehavioursFromLibrary<T>();
        }

        /// <summary>
        /// FindObectOfType<UnityObject> Alternative with Interface support
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T FindObjectFromLibraryById<T>(ComponentId behaviourId)
        {
            CheckForSystemLibNull();
            return _myLibrary.GetBehaviourFromLibraryById<T>(behaviourId.objBt);
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T FindObjectFromOtherSceneLibrary<T>(string sceneId)
        {
            CheckForSystemLibNull();
            return _myLibrary.GetComponentFromOtherSceneLibrary<T>(sceneId);
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene by ID
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T FindObjectFromOtherSceneLibraryById<T>(ComponentId behaviourId, string sceneId)
        {
            CheckForSystemLibNull();
            return _myLibrary.GetComponentFromOtherSceneLibraryById<T>(behaviourId, sceneId);
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene by ID
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public IReadOnlyList<T> FindObjectsFromOtherSceneLibrary<T>(string sceneId)
        {
            CheckForSystemLibNull();
            return _myLibrary.GetComponentsFromOtherSceneLibrary<T>(sceneId);
        }
        #endregion

        #region STATIC_COMPONENT_GETTERS
        /// <summary>
        /// GetComponent<T> Performance Alternative
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public static T GetComponentFromGameObjectOf<T>(CustomBehaviour targetBehaviour)
        {
            CheckForStaticSystemLibNull();
            int instanceId = targetBehaviour.id._goInsId;
            return _myLibrary.GetBehaviourOfGameObject<T>(instanceId);
        }

        /// <summary>
        /// FindObjectOfType<T> Performance Alternative
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>

        public static IReadOnlyList<T> GetComponentsFromGameObjectOf<T>(CustomBehaviour targetBehaviour)
        {
            CheckForStaticSystemLibNull();
            int instanceId = targetBehaviour.id._goInsId;
            return _myLibrary.GetBehavioursOfGameObject<T>(instanceId);
        }

        /// <summary>
        /// FindObjectOfType<T> Performance Alternative
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public static T FindObjFromLibrary<T>()
        {
            CheckForStaticSystemLibNull();
            return _myLibrary.GetBehaviourFromLibrary<T>();
        }

        /// <summary>
        /// FindObjectsOfType Performance Alternative 
        /// </summary>
        /// <typeparam name="T"> Class, Interface </typeparam>
        /// <returns>List<typeparamref name="T"/> of Requested BehaviourType </returns>
        public static IReadOnlyList<T> FindObjsFromLibrary<T>()
        {
            CheckForStaticSystemLibNull();
            return _myLibrary.GetBehavioursFromLibrary<T>();
        }

        /// <summary>
        /// FindObectOfType<UnityObject> Alternative with Interface support
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public static T FindObjFromLibraryById<T>(ComponentId behaviourId)
        {
            CheckForStaticSystemLibNull();
            return _myLibrary.GetBehaviourFromLibraryById<T>(behaviourId.objBt);
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public static T FindObjFromOtherSceneLibrary<T>(string sceneId)
        {
            CheckForStaticSystemLibNull();
            return _myLibrary.GetComponentFromOtherSceneLibrary<T>(sceneId);
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene by ID
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public static T FindObjFromOtherSceneLibraryById<T>(ComponentId behaviourId, string sceneId)
        {
            CheckForStaticSystemLibNull();
            return _myLibrary.GetComponentFromOtherSceneLibraryById<T>(behaviourId, sceneId);
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene by ID
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public static IReadOnlyList<T> FindObjsFromOtherSceneLibrary<T>(string sceneId)
        {
            CheckForStaticSystemLibNull();
            return _myLibrary.GetComponentsFromOtherSceneLibrary<T>(sceneId);
        }
        #endregion

        #region INSTANTIATORS_DESTRUCTORS
       
        public void CacheReference<T>(T newObject,bool cacheEntireHierarchy) where T : CustomBehaviour
        {
            CheckForSystemLibNull();

            if (cacheEntireHierarchy)
            {
                CustomBehaviour[] behaviours = newObject.GetComponentsInChildren<CustomBehaviour>(true);
                for (int i = 0; i < behaviours.Length; i++)
                    _myLibrary.AddBehaviour(behaviours[i]);

                return;
            }
            
            _myLibrary.AddBehaviour(newObject);
        }

        public void DeCacheReference<T>(T targetObject, bool deCacheEntireHierarchy) where T : CustomBehaviour 
        {
            if (deCacheEntireHierarchy)
            {
                CustomBehaviour[] behaviours = targetObject.GetComponentsInChildren<CustomBehaviour>(true);
                for (int i = 0; i < behaviours.Length; i++)
                {
                    _myLibrary.CleanReferencesExplicitlyOf(behaviours[i]);
                }
                
                return;
            }

            _myLibrary.CleanReferencesExplicitlyOf(targetObject);
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
            if (destoryGameObject)
            {
                DeCacheReference(original, true);
                DestroyImmediate(go);
                return;
            }

            DestroyImmediate(original);
            DeCacheReference(original, false);

        }

        public T GetPooled<T>() 
        {
            CheckForSystemLibNull();
            return _myLibrary.GetPooled<T>();
        }

        public void Pool(CustomBehaviour behaviour) 
        {
            CheckForSystemLibNull();
            _myLibrary.Pool(behaviour);
        }

        public new T Instantiate<T>(T original) where T : CustomBehaviour
        {
            T newBehaviour = MonoInstantiate(original);
            CacheReference(newBehaviour,true);
            return newBehaviour;
        }

        public T Instantiate<T>(T original,ComponentId id) where T : CustomBehaviour
        {
            T newBehaviour = MonoInstantiate(original);
             original.id = id;
            CacheReference(newBehaviour,true);
            return newBehaviour;
        }

        public new T Instantiate<T>(T original, Transform parent) where T : CustomBehaviour
        {
            T newBehaviour = MonoInstantiate(original, parent);
            CacheReference(newBehaviour,true);
            return newBehaviour;
        }

        public new T Instantiate<T>(T original, Transform parent, bool instantiateInWorldSpace) where T : CustomBehaviour
        {
            T newBehaviour = MonoInstantiate(original, parent,instantiateInWorldSpace);
            CacheReference(newBehaviour,true);
            return newBehaviour;
        }

        public new T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : CustomBehaviour
        {
            T newBehaviour = MonoInstantiate(original, position, rotation);
            CacheReference(newBehaviour,true);
            return newBehaviour;
        }
        
        public new T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : CustomBehaviour
        {
            T newBehaviour = MonoInstantiate(original, position, rotation,parent);
            CacheReference(newBehaviour,true);
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
        static void CheckForStaticSystemLibNull()
        {
            if (_myLibrary == null)
            {
                throw new Exception($"Scene library instance not found, Make sure to scan behaviours from scene library in editor or isntantiate from CustomBehaviour");
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
