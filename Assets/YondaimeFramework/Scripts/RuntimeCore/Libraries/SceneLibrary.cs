using System.Collections.Generic;
using UnityEngine;
using System;

namespace YondaimeFramework
{
    public class SceneLibrary : MonoBehaviour
    {
        #region PUBLIC_VARS
        [SerializeField] private LibraryType _libraryType;
        [SerializeField] private SceneId _systemId;
        [SerializeField] public CustomBehaviour[] _behaviours;
        public string SceneId
        {
            get
            {
                return _systemId.id;
            }
        }
        #endregion


        #region LIBRARY_REFERENCES
        private ILibrary _library;
        private RootLibrary _rootLibrary;
        #endregion


        #region INITIALIZERS
        private void Awake()
        {
            _library = LibraryFactory.ConstructLibrary(_libraryType);
            RegisterSelfInRootLibrary();
            GenerateBehaviourLookups();
        }

        private void OnDestroy()
        {
            DeregisterSelfFromRootLibrary();
        }
        #endregion


        #region LOOKUP_GENERATION
        public void GenerateBehaviourLookups()
        {
            CustomBehaviour[] behaviours  = _behaviours;

            Dictionary<Type, List<CustomBehaviour>> _behaviourLookup = new Dictionary<Type, List<CustomBehaviour>>();
            Dictionary<int, List<CustomBehaviour>> _idLookup = new Dictionary<int, List<CustomBehaviour>>();
            CheckForEmptyBehaviours();
            GenerateBehaviourLookUp();
            _library.InitLibrary(_behaviourLookup, _idLookup);

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
           return _library.GetBehaviourFromLibrary<T>();
        }

        public  T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
           return _library.GetBehaviourOfGameObject<T>(requesteeGameObjectInstanceId);
        }

        public  T GetBehaviourFromLibraryById<T>(int behaviourId)
        {
           return _library.GetBehaviourFromLibraryById<T>(behaviourId);
        }

        public List<T> GetBehavioursFromLibrary<T>()
        {
           return _library.GetBehavioursFromLibrary<T>();
        }

        public List<T> GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
           return _library.GetBehavioursOfGameObject<T>(requesteeGameObjectInstanceId);
        }

      
        #endregion


        #region ALLOCATORS
        public void AddBehaviour<T>(T newBehaviour) where T:CustomBehaviour
        {
            newBehaviour.SetLibrary(this);
            _library.AddBehaviour(newBehaviour);
        }

        
        public void CleanReferencesFor(CustomBehaviour customBehaviour)
        {
           _library.CleanReferencesFor(customBehaviour);
        }

        public void AddBehaviours<T>(List<T> newBehaviours) where T : CustomBehaviour
        {
            int count = newBehaviours.Count;
            for (int i = 0; i < count; i++)
                newBehaviours[i].SetLibrary(this);

            _library.AddBehaviours(newBehaviours);
        }

        public void AddBehaviours<T>(T[] newBehaviours) where T : CustomBehaviour
        {
            int count = newBehaviours.Length;
            for (int i = 0; i < count; i++)
                newBehaviours[i].SetLibrary(this);

            _library.AddBehaviours<T>(newBehaviours);
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
            _rootLibrary.RemoveFromLibrary(SceneId);
        }

        public SceneLibrary GetSceneLibraryFromRootLibraryById(string systemId)
        {
            return _rootLibrary.GetSceneLibraryById(systemId);
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
        public void LogBehvLookup() => _library.LogLookup();
        public void LogIdLookuip() => _library.LogIdLookup();
        #endregion


       

    }

    public enum LibraryType
    {
        Standard,
        HighPerformance,
        Pooled
    }

    public static class LibraryFactory
    {
        public static ILibrary ConstructLibrary(LibraryType libraryType)
        {
            switch (libraryType)
            {
                case LibraryType.Standard:
                    return ConstructStandardLibrary();

                 // case LibraryType.STATIC_PERFORMANT:
                 // case LibraryType.POOLED:
            }
            return default;
        }

        private static StandardBehaviourLibrary ConstructStandardLibrary()
        {
            return new StandardBehaviourLibrary();
        }
        
        private static PooledBehaviourLibrary ConstructPooledLibrary()
        {
            return new PooledBehaviourLibrary();
        }


    }

}