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
        TypeLookUp _typeLookUp = new TypeLookUp();
        private Dictionary<int, TypeLookUp> _idLookup = new Dictionary<int, TypeLookUp>();
        private Dictionary<int, TypeLookUp> _goLookup = new Dictionary<int, TypeLookUp>();

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
        internal void LogLookup<T,K>(Dictionary<T,K> dict,string name) where K: IList
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
                    v += $" {((CustomBehaviour)item.Value[i]).gameObject.name} -- {((CustomBehaviour)item.Value[i]).id.objBt}. \n"; 
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
                _typeLookUp.GenerateLookUp(behaviours);
            }
            void GenerateIdLookup()
            {
            
                for (int i = 0; i < behaviours.Length; i++)
                {
                    CustomBehaviour behaviour = behaviours[i];
                    behaviour.RefreshIds();
                    AddToGoLookup(behaviour);
                    if(HasCustomId(behaviour))
                        AddToIdLookup(behaviour);
                } 
            
            }
            bool HasCustomId(CustomBehaviour behaviour)
            {
                return behaviour.id.objBt != ComponentId.None;
            }
            void AddToIdLookup(CustomBehaviour behaviour)
            {
                int id = behaviour.id.objBt;

                if (!_idLookup.ContainsKey(id))
                    _idLookup.Add(id, new TypeLookUp());

                _idLookup[id].AddBehaviour(behaviour);
            }
            void AddToGoLookup(CustomBehaviour behaviour) 
            {
                int id = behaviour.id._goInsId;

                if (!_goLookup.ContainsKey(id))
                    _goLookup.Add(id, new TypeLookUp());

                _goLookup[id].AddBehaviour(behaviour);
            }
        }

        #endregion

        #region COMPONENT_GETTERS

        public T GetBehaviourFromLibrary<T>()
        {
            return _typeLookUp.GetBehaviour<T>();
        }

        public  T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
            return _goLookup[requesteeGameObjectInstanceId].GetBehaviour<T>();
        }

        public  T GetBehaviourFromLibraryById<T>(int behaviourId)
        {
            MissingIdExceptionCheck(behaviourId);
            return _idLookup[behaviourId].GetBehaviour<T>();
        }

        public IReadOnlyList<T> GetBehavioursFromLibrary<T>()
        {
            return _typeLookUp.GetBehavioursFromContainer<T>();
        }

        public IReadOnlyList<T> GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
            return _goLookup[requesteeGameObjectInstanceId].GetBehavioursFromContainer<T>();
        }

        public T GetComponentFromOtherSceneLibrary<T>(string sceneId)
        {
            return _rootLibrary.GetSceneLibraryById(sceneId).GetBehaviourFromLibrary<T>();
        }

        public T GetComponentFromOtherSceneLibraryById<T>(ComponentId behaviourId, string sceneId)
        {
            return _rootLibrary.GetSceneLibraryById(sceneId).GetBehaviourFromLibraryById<T>(behaviourId.objBt);
        }

        public IReadOnlyList<T> GetComponentsFromOtherSceneLibrary<T>(string sceneId)
        {
            return _rootLibrary.GetSceneLibraryById(sceneId).GetBehavioursFromLibrary<T>();
        }

        #endregion

        #region ALLOCATOR_HANDLES

        public void AddBehaviour<T>(T newBehaviour)
        {
            CustomBehaviour behv = (CustomBehaviour)(object)newBehaviour;
            behv.SetLibrary(this);
            behv.RefreshIds();

            _typeLookUp.AddBehaviour(behv);
            AddToGoLookup(behv);
            CheckAndAddToIdLookup(behv);
        }
        private void CheckAndAddToIdLookup(CustomBehaviour newBehaviour)
        {
            newBehaviour.RefreshIds();
            int id = newBehaviour.id.objBt;

            if (id == ComponentId.None)
                return;

            if(!_idLookup.ContainsKey(id))   
                _idLookup.Add(id, new TypeLookUp());

            _idLookup[id].AddBehaviour(newBehaviour);
        }

        private void AddToGoLookup(CustomBehaviour newBehaviour) 
        {
            int id = newBehaviour.id._goInsId;
            if (!_goLookup.ContainsKey(id))
                _goLookup.Add(id, new TypeLookUp());

            _goLookup[id].AddBehaviour(newBehaviour);
        }


        public void CleanNullReferencesFor(ComponentId id,Type t) 
        {
            _typeLookUp.CleanNullReferencesFor(t);
            _goLookup[id._goInsId].CleanNullReferencesFor(t);

            int cid = id.objBt;
            if(cid != ComponentId.None)
                _idLookup[cid].CleanNullReferencesFor(t);

        }
               
        public void LogIdLookup()
        {
            foreach (var item in _idLookup)
            {
                LogLookup(item.Value.lookup, $"Id Lookup {item.Key}");
            }
        }
        
        public void LogGOLookup()
        {
            foreach (var item in _goLookup)
            {
                LogLookup(item.Value.lookup, $"GO Lookup {item.Key}");
            }
        }

        public void LogLookup()
        {
            LogLookup(_typeLookUp.lookup,"Behv Lookup");
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
                _idLookup[oldId].CleanReferencesExplicitlyOf(behaviour,behaviour.GetType());
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
            if (behaviours.Length == behv.Length)
                return;

             behaviours = behv;
            _typeLookUp = new TypeLookUp();
            _idLookup = new Dictionary<int, TypeLookUp>();
            _goLookup = new Dictionary<int, TypeLookUp>();

             GenerateLookups();
        }


        #endregion

        #region EXCEPTIONS
        void MissingIdExceptionCheck(int t) 
        {
            if (!_idLookup.ContainsKey(t))
                throw new Exception($"No component of id {t} present in lookup, Make sure to scan library once or instantiate via CustomBehaviour");
        }

        #endregion

    }
}
