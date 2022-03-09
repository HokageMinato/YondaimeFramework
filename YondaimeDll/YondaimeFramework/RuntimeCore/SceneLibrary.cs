using System.Collections.Generic;
using UnityEngine;
using System;

namespace YondaimeFramework
{
    public class SceneLibrary : MonoBehaviour
    {
        #region PUBLIC_VARS
        [SerializeField] private CustomBehaviour[] _behaviours;
        [SerializeField] private SceneId _systemId;
        public string SystemId
        {
            get
            {
                return _systemId.id;
            }
        }
        #endregion



        #region LIBRARY_REFERENCES
        private StandardBehaviourLibrary _standardBehaviourLibrary = new StandardBehaviourLibrary();
        private RootLibrary _rootLibrary;
        #endregion




        #region UNITY_CALLBACKS
        private void Start()
        {
            GenerateBehaviourLookups(_standardBehaviourLibrary.InitLibrary);
        }

        private void Awake()
        {
            FrameworkLogger.Log($"Init Scenen library {_systemId.id}");
            RegisterSelfInRootLibrary();
        }

        private void OnDestroy()
        {
            DeregisterSelfFromRootLibrary();
        }

        #endregion



        #region LOOKUP_GENERATION
        public void GenerateBehaviourLookups(Action<Dictionary<Type, List<CustomBehaviour>>,Dictionary<int, List<CustomBehaviour>>> OnGenerated)
        {
            CustomBehaviour[] behaviours  = _behaviours;

            Dictionary<Type, List<CustomBehaviour>> _behaviourLookup = new Dictionary<Type, List<CustomBehaviour>>();
            Dictionary<int, List<CustomBehaviour>> _idLookup = new Dictionary<int, List<CustomBehaviour>>();
            CheckForEmptyBehaviours();
            SetSceneLibraryReference();
            GenerateBehaviourLookUp();
            OnGenerated(_behaviourLookup,_idLookup);

            void CheckForEmptyBehaviours()
            {
                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (behaviours[i] == null)
                        throw new Exception("Null object detected,Make sure to Scan library after making all scene edits");
                    
                }
            }
            void GenerateBehaviourLookUp()
            {
                for (int i = 0; i < behaviours.Length; i++)
                {
                    CustomBehaviour currentBehaviour = behaviours[i];
                    Type currentBehaviourType = currentBehaviour.GetType();
                    AddToBehaviourLookup(currentBehaviour, currentBehaviourType);
                    AddBehaviourInterfacesInLookup(currentBehaviour, currentBehaviourType);
                    if (HasCustomId(currentBehaviour))
                    {
                        AddToIdLookup(currentBehaviour);
                    }
                }
            }
            bool HasCustomId(CustomBehaviour behaviour) 
            {
                behaviour.RefreshIds();
                return behaviour.id.objBt != ComponentId.None;
            }
            void SetSceneLibraryReference() 
            {

                for (int i = 0; i < behaviours.Length; i++)
                {
                    behaviours[i].SetLibrary(this);
                }
            }
            void AddBehaviourInterfacesInLookup(CustomBehaviour behaviour, Type t)
            {
                Type[] itypes = t.GetInterfaces();
                for (int i = 0; i < itypes.Length; i++)
                {
                    AddToBehaviourLookup(behaviour, itypes[i]);
                }
            }
            void AddToBehaviourLookup(CustomBehaviour behaviour, Type t)
            {
                if (!_behaviourLookup.ContainsKey(t))
                {
                    _behaviourLookup.Add(t, new List<CustomBehaviour>() { behaviour });
                    return;
                }

                _behaviourLookup[t].Add(behaviour);
            }
            void AddToIdLookup(CustomBehaviour behaviour)
            {
                int id = behaviour.id.objBt;

                if (!_idLookup.ContainsKey(id))
                {
                    _idLookup.Add(id, new List<CustomBehaviour>() { behaviour });
                    return;
                }

                _idLookup[id].Add(behaviour);
            }
            
        }
        #endregion



        #region COMPONENT_GETTER_HANDLES

        public T GetBehaviourFromLibrary<T>()
        {
           return _standardBehaviourLibrary.GetBehaviourFromLibrary<T>();
        }

        public  T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
           return _standardBehaviourLibrary.GetBehaviourOfGameObject<T>(requesteeGameObjectInstanceId);
        }

        public  T GetBehaviourFromLibraryById<T>(int behaviourId)
        {
           return _standardBehaviourLibrary.GetBehaviourFromLibraryById<T>(behaviourId);
        }

        public List<T> GetBehavioursFromLibrary<T>()
        {
          return _standardBehaviourLibrary.GetBehavioursFromLibrary<T>();
        }

        public List<T> GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
           return _standardBehaviourLibrary.GetBehavioursOfGameObject<T>(requesteeGameObjectInstanceId);
        }

        public SceneLibrary GetSceneLibraryFromRootLibraryById(string systemId)
        {
           return _rootLibrary.GetSceneLibraryById(systemId);
        }


        #endregion




        #region ALLOCATORS
        public void AddBehaviour<T>(T newBehaviour) where T:CustomBehaviour
        {
            newBehaviour.SetLibrary(this);
            _standardBehaviourLibrary.AddBehaviour(newBehaviour);
        }

        public void AddBehaviours<T>(List<T> newBehaviours) where T : CustomBehaviour
        {
            for (int i = 0; i < newBehaviours.Count; i++)
                SetLibraryToBehaviour(newBehaviours[i]);

           _standardBehaviourLibrary.AddBehaviours(newBehaviours);
        }

        public void AddBehaviours<T>(T[] newBehaviours) where T : CustomBehaviour
        {
            for (int i = 0; i < newBehaviours.Length; i++)
                SetLibraryToBehaviour(newBehaviours[i]);

            _standardBehaviourLibrary.AddBehaviours<T>(newBehaviours);    
        }

        public void CleanReferencesFor(CustomBehaviour customBehaviour)
        {
          _standardBehaviourLibrary.CleanReferencesFor(customBehaviour);
        }

        private void SetLibraryToBehaviour(CustomBehaviour behaviour) 
        { 
            behaviour.SetLibrary(this);
        }
        #endregion


        #region ROOT_LIBRARY_INTERACTORS
        private void RegisterSelfInRootLibrary()
        {

            RootLibraryNonExistanceCheck();
            _rootLibrary = RootLibrary.Instance;
            _rootLibrary.AddSceneLibrary(this);
        }

        private void DeregisterSelfFromRootLibrary()
        {
            _rootLibrary.RemoveFromLibrary(SystemId);
        }

        #endregion


        #region EXCEPTIONS
        private void RootLibraryNonExistanceCheck()
        {
            if (RootLibrary.Instance == null)
                throw new Exception("RootLibrary instance not found, either create one or check script execution order for Root Library to execute before scene library");
        }

        #endregion

        #region EDITOR_HELPERS
        public void SetBehaviours(CustomBehaviour[] behaviours) 
        { 
            _behaviours = behaviours;
        }

        public void LogBehvLookup() => _standardBehaviourLibrary.LogLookup();
        public void LogIdLookuip() => _standardBehaviourLibrary.LogIdLookup();
        #endregion
    }



}