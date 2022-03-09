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
        [SerializeField] private SceneLibrary _mySceneLibrary;
        #endregion

        #region IDS
        public int GOInstanceId { get { return id._goInsId; } }

        public ComponentId id;

        public int PoolState;
        #endregion

        #region LIBRARY_HANDLES
        public void RefreshIds()
        {
            id._goInsId = gameObject.GetInstanceID();
        }

        public void SetLibrary(SceneLibrary sceneLibrary)
        {
            _mySceneLibrary = sceneLibrary;
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
            int instanceId = id._goInsId;
            return _mySceneLibrary.GetBehaviourOfGameObject<T>(instanceId);
        }

        /// <summary>
        /// FindObjectOfType<T> Performance Alternative
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>

        public List<T> GetComponentsFromMyGameObject<T>()
        {
            int instanceId = id._goInsId;
            return _mySceneLibrary.GetBehavioursOfGameObject<T>(instanceId);
        }

        /// <summary>
        /// FindObjectOfType<T> Performance Alternative
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromLibrary<T>()
        {
            return _mySceneLibrary.GetBehaviourFromLibrary<T>();
        }

        /// <summary>
        /// FindObjectsOfType Performance Alternative 
        /// </summary>
        /// <typeparam name="T"> Class, Interface </typeparam>
        /// <returns>List<typeparamref name="T"/> of Requested BehaviourType </returns>
        public List<T> GetComponentsFromLibrary<T>()
        {
            return _mySceneLibrary.GetBehavioursFromLibrary<T>();
        }

        /// <summary>
        /// FindObectOfType<UnityObject> Alternative with Interface support
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromLibraryById<T>(ComponentId behaviourId)
        {
            return _mySceneLibrary.GetBehaviourFromLibraryById<T>(behaviourId.objBt);
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromOtherSceneLibrary<T>(string sceneId)
        {
            return _mySceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehaviourFromLibrary<T>();
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene by ID
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public T GetComponentFromOtherSceneLibraryById<T>(ComponentId behaviourId, string sceneId)
        {
            return _mySceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehaviourFromLibraryById<T>(behaviourId.objBt);
        }


        /// <summary>
        /// Singleton Alternative to fetch from other scene by ID
        /// </summary>
        /// <typeparam name="T">Class,Interface</typeparam>
        /// <returns>Requested BehaviourType <typeparamref name="T"/> </returns>
        public List<T> GetComponentsFromOtherSceneLibrary<T>(string sceneId)
        {
            
            return _mySceneLibrary.GetSceneLibraryFromRootLibraryById(sceneId).GetBehavioursFromLibrary<T>();
        }
        #endregion
       
        #region INSTANTIATORS_DESTRUCTORS
        private void _Instantiate<T>(T newObject) where T : CustomBehaviour
        {
            _mySceneLibrary.AddBehaviour(newObject);
        }

        private void _SetId<T>(T newObject,ComponentId id) where T : CustomBehaviour 
        { 
            newObject.id=id;
        }
        #endregion


        #region STATIC_CONSTRUCT_DESTRUCT_HANDLES
        public void DestroyCustom<T>(T original,bool destoryGameObject=false) where T : CustomBehaviour 
        {
            GameObject go = original.gameObject;
            DestroyImmediate(original);

            if (destoryGameObject)
                DestroyImmediate(go);


            
            _mySceneLibrary.CleanReferencesFor(original);
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
            _SetId(newBehaviour,id);
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
            if (_mySceneLibrary == null)
            {
                throw new Exception($"Scene library not assigned at ({name}) Make sure to scan behaviours from scene library in editor");
            }
        }
        #endregion


    #if UNITY_EDITOR
        #region EDITOR

        public SceneLibrary ml => _mySceneLibrary;

        public static int GenerateHashCode(string val)
        {
            return val.GetHashCode() * 92821;  // PRIME = 92821 or another prime number.
        }
        #endregion


    }
    #endif
}
