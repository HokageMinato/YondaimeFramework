using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    public class StandardBehaviourLibrary :MonoBehaviour, ILibrary
    {

        #region COMPONENTS
        [SerializeField] private SceneId sceneId;
        [SerializeField] private CustomBehaviour[] behaviours;
        #endregion

        #region LOOKUPS
        TypeLookUp typeLookUp = new TypeLookUp();
        private Dictionary<int, TypeLookUp> _idMap = new Dictionary<int, TypeLookUp>();
        RootLibrary _rootLibrary;

        public SceneId SceneId => sceneId;
        #endregion

        #region INITIALIZERS

        public void Awake()
        {
            _rootLibrary = RootLibrary.Instance;
            RootLibraryExistanceCheck();
            RegisterSelfInRootLibrary();
            GenerateLookups();
        }

        public void OnDestroy()
        {
            DeregisterSelfFromRootLibrary();
        }

        private void RegisterSelfInRootLibrary()
        {
            _rootLibrary = RootLibrary.Instance;
            _rootLibrary.AddSceneLibrary(this);
        }

        private void DeregisterSelfFromRootLibrary()
        {
            _rootLibrary.RemoveFromLibrary(SceneId);
        }
        private void RootLibraryExistanceCheck()
        {
            if (RootLibrary.Instance == null)
                throw new Exception("RootLibrary instance not found, either create one or check script execution order for Root Library to execute before scene library");
        }
        internal void LogLookup<T,K>(Dictionary<T,K> dict,string name) where K: List<CustomBehaviour>
        {
            

            string val = $"Showinggg => {name} <=  ";
            foreach (KeyValuePair<T, K> item in dict)
            {
                string v = "";
                for (int i = 0; i < item.Value.Count; i++)
                {
                    if (item.Value[i] == null)
                    {
                        Debug.LogError("UNCLEAN REF");
                    }
                    v += $" {item.Value[i].gameObject.name} -- {item.Value[i].id.objBt} --- {item.Value[i].GetInstanceID()}. \n"; 
                }

                
                val += $"Type { item.Key}, TotalInstances { item.Value.Count} GOS =>[ {v} ] \n\n\n";
            }
            Debug.Log(val);
        }

        private void GenerateLookups()
        {
            PrepareBehaviours();
            GenerateBehaviourLookup();
            GenerateIdLookup();

            void PrepareBehaviours()
            {
                for (int i = 0; i < behaviours.Length; i++)
                {
                    CustomBehaviour customBehaviour = behaviours[i];
                    customBehaviour.SetLibrary(this);
                    customBehaviour.RefreshIds();
                }
            }
            void GenerateBehaviourLookup()
            {
                typeLookUp.GenerateLookUp(behaviours);
            }
            void GenerateIdLookup()
            {
            
                for (int i = 0; i < behaviours.Length; i++)
                {
                    CustomBehaviour behaviour = behaviours[i];
                    if(HasCustomId(behaviour))
                        AddToIdLookup(behaviour);
                } 
            
            }
            bool HasCustomId(CustomBehaviour behaviour)
            {
                behaviour.RefreshIds();
                return behaviour.id.objBt != ComponentId.None;
            }
            void AddToIdLookup(CustomBehaviour behaviour)
            {
                int id = behaviour.id.objBt;

                if (!_idMap.ContainsKey(id))
                    _idMap.Add(id, new TypeLookUp());

                _idMap[id].AddBehaviour(behaviour);
            }
        }

        #endregion

        #region COMPONENT_GETTERS

        public T GetBehaviourFromLibrary<T>()
        {
            return typeLookUp.GetBehaviour<T>();
        }

        public  T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
            return typeLookUp.GetBehaviourOfGameObject<T>(requesteeGameObjectInstanceId);
        }

        public  T GetBehaviourFromLibraryById<T>(int behaviourId)
        {
            MissingIdExceptionCheck(behaviourId);
            return _idMap[behaviourId].GetBehaviour<T>();
        }

        public List<T> GetBehavioursFromLibrary<T>()
        {
            return typeLookUp.GetBehavioursFromLibrary<T>();
        }

        public List<T> GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
            return typeLookUp.GetBehavioursOfGameObject<T>(requesteeGameObjectInstanceId);
        }

        public T GetComponentFromOtherSceneLibrary<T>(string sceneId)
        {
            return _rootLibrary.GetSceneLibraryById(sceneId).GetBehaviourFromLibrary<T>();
        }

        public T GetComponentFromOtherSceneLibraryById<T>(ComponentId behaviourId, string sceneId)
        {
            return _rootLibrary.GetSceneLibraryById(sceneId).GetBehaviourFromLibraryById<T>(behaviourId.objBt);
        }

        public List<T> GetComponentsFromOtherSceneLibrary<T>(string sceneId)
        {
            return _rootLibrary.GetSceneLibraryById(sceneId).GetBehavioursFromLibrary<T>();
        }

        #endregion

        #region ALLOCATOR_HANDLES

        public void AddBehaviour<T>(T newBehaviour)
        {
            typeLookUp.AddBehaviour(newBehaviour);
            CheckAndAddToIdLookup(newBehaviour);
        }

        private void CheckAndAddToIdLookup<T>(T newBehaviour)
        {
            CustomBehaviour behv = (CustomBehaviour)(object)newBehaviour;
            behv.RefreshIds();
            int id = behv.id.objBt;

            if (id == ComponentId.None)
                return;

            if(!_idMap.ContainsKey(id))   
                _idMap.Add(id, new TypeLookUp());

            _idMap[id].AddBehaviour(newBehaviour);
        }

        public void CleanNullReferencesFor<T>(int id) 
        {
            typeLookUp.CleanNullReferencesFor<T>();

            if(id != ComponentId.None)
                _idMap[id].CleanNullReferencesFor<T>();

        }
        
        public void LogIdLookup()
        {
            foreach (var item in _idMap)
            {
                LogLookup(item.Value.lookup, $"Id: {item.Key}");
            }
        }

        public void LogLookup()
        {
            LogLookup(typeLookUp.lookup,"Behv Lookup");
        }

        public void SetComponentId(CustomBehaviour behaviour, ComponentId newId) 
        { 
            ChangeIdRefFor(behaviour,newId);
        }

        #endregion


        #region INTERNAL_ALLOCATION_WORKERS

        private void ChangeIdRefFor(CustomBehaviour behaviour, ComponentId newId) 
        {
            int oldId = behaviour.id.objBt;
            if (oldId == ComponentId.None)
            {
                behaviour.id = newId;
                CheckAndAddToIdLookup(behaviour);
            }
            else 
            {
                _idMap[oldId].CleanNullReferencesFor(behaviour.GetType());
                behaviour.id = newId;
                CheckAndAddToIdLookup(behaviour);
            }
        }


        public T GetPooled<T>()
        {
            throw new Exception("No components are pooled natively in Standard Library");
        }

        public void Pool(CustomBehaviour behaviour)
        {
            throw new Exception("Pooling components natively in Standard Library is Not Allowed");
        }

        public void SetBehaviours(CustomBehaviour[] behv)
        {
            behaviours = behv;
        }


        #endregion

        #region EXCEPTIONS
        void MissingIdExceptionCheck(int t) 
        {
            if (!_idMap.ContainsKey(t))
                throw new Exception($"No component of id {t} present in lookup, Make sure to scan library once or instantiate via CustomBehaviour");
        }

        #endregion

    }
}
