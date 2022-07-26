using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YondaimeFramework
{

    public class PooledBehaviourLibrary : MonoBehaviour, ILibrary
    {
        #region PARAMETERS
        private const int Pooled = 1, UnPooled = 0 , Managed = 0, UnManaged = 1;
        [SerializeField] private SceneId sceneId;
        [SerializeField] private ExecutionMode executionMode;
        private int modeFlag;
        #endregion

        #region COMPONENTS
        [SerializeField] private CustomBehaviour[] behaviours;
        #endregion

        #region LOOKUPS
        TypeLookUp _typeLookUp = new TypeLookUp();
        private Dictionary<int, TypeLookUp> _idLookup = new Dictionary<int, TypeLookUp>();
        private Dictionary<int, TypeLookUp> _goLookup = new Dictionary<int, TypeLookUp>();
        private Dictionary<Type, PerformancePool<CustomBehaviour>> _pool = new Dictionary<Type, PerformancePool<CustomBehaviour>>();
        RootLibrary _rootLibrary;
        #endregion

        #region PUBLIC_ACCESSORS        
        public SceneId SceneId => sceneId;
        #endregion

        #region INITIALIZERS

        public void Awake()
        {
            _rootLibrary = RootLibrary.Instance;
            RootLibraryMissingExceptionCheck();
            RegisterSelfInRootLibrary();
            GenerateBehaviourLookups(behaviours);
            SetExecutionMode();
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

        

        internal void LogLookup<T, K>(Dictionary<T, K> dict, string name) where K : List<CustomBehaviour>
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


        private void GenerateBehaviourLookups(CustomBehaviour[] behaviourss)
        {

            PrepareBehaviours();
            GenerateBehaviourLookup();
            GenerateIdLookup();


            void PrepareBehaviours()
            {
                for (int i = 0; i < behaviourss.Length; i++)
                {
                    CustomBehaviour customBehaviour = behaviourss[i];
                    customBehaviour.SetLibrary(this);
                    customBehaviour.RefreshIds();
                }
            }
            void GenerateBehaviourLookup()
            {
                _typeLookUp.GenerateLookUp(behaviourss);
            }
            void GenerateIdLookup()
            {

                for (int i = 0; i < behaviourss.Length; i++)
                {
                    CustomBehaviour behaviour = behaviourss[i];
                    behaviour.RefreshIds();
                    AddToGoLookup(behaviour);
                    if (HasCustomId(behaviour))
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

        private void SetExecutionMode() 
        {
            modeFlag = (executionMode == ExecutionMode.MANAGED_REFERENCES) ? Managed : UnManaged; 
        }

        public void SyncLibrary(CustomBehaviour[] syncData)
        {
            GenerateBehaviourLookups(syncData);
        }

        public CustomBehaviour[] GetBehaviourSyncData()
        {
            return behaviours;
        }

        public void PurgeLibrary()
        {
            PurgeLookups(_typeLookUp);

            foreach (KeyValuePair<int, TypeLookUp> item in _goLookup)
                PurgeLookups(item.Value);

            foreach (KeyValuePair<int, TypeLookUp> item in _idLookup)
                PurgeLookups(item.Value);
            
            PurgePools();
                            
                Debug.Log("Purged");

        }

        private void PurgeLookups(TypeLookUp lookup)
        {
            lookup.CleanAllNullReferencesOnSceneChange();
        }

        private void PurgePools() 
        {
            foreach (KeyValuePair<Type, PerformancePool<CustomBehaviour>> item in _pool)
            {
                item.Value.CleanNullReferences();
            }
        }
        #endregion

        #region COMPONENT_GETTERS

        public T GetBehaviourFromLibrary<T>()
        {
            return _typeLookUp.GetBehaviour<T>();
        }

        public T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
            return _goLookup[requesteeGameObjectInstanceId].GetBehaviour<T>();
        }

        public T GetBehaviourOfGameObjectSafe<T>(int requesteeGameObjectInstanceId)
        {
            if (DoesIdExist(requesteeGameObjectInstanceId, _goLookup))
                return _goLookup[requesteeGameObjectInstanceId].GetBehaviour<T>();

            return default;
        }


        public T GetBehaviourFromLibraryById<T>(int behaviourId)
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

        public T GetPooled<T>()
        {
            Type type = typeof(T);

            if (MissingPoolExceptionCheck(type)) 
            {
                Debug.Log($"Pool of  type {type} is empty, Make sure you pool elements before de-pooling elements");
                return default;
            }

            CustomBehaviour obj = _pool[type].GetPooled();
            obj.poolState = UnPooled;
            obj.gameObject.SetActive(true);

            T pooledObj = (T)(object)obj;

            if (modeFlag == Managed)
                AddBehaviour(pooledObj);

            return pooledObj;

        }

        public void Pool(CustomBehaviour behaviour)
        {
            NullElementPoolExceptionCheck(behaviour);

            Type type = behaviour.GetType();
            if (!_pool.ContainsKey(type))
                _pool.Add(type, new PerformancePool<CustomBehaviour>());

            behaviour.poolState = Pooled;
            behaviour.gameObject.SetActive(false);
            _pool[type].Pool(behaviour);

            if (modeFlag == Managed)
                CleanPooledReferencesFor(behaviour);

            behaviour.OnPooled();
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

            if (!_idLookup.ContainsKey(id))
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

      

        public void CleanPooledReferencesFor(CustomBehaviour customBehaviour)
        {
            int go = customBehaviour.id._goInsId;

            _typeLookUp.CleanReferencesExplicitlyOf(customBehaviour);
            _goLookup[go].CleanReferencesExplicitlyOf(customBehaviour);

            int id = customBehaviour.id.objBt;
            if (id != ComponentId.None)
            {
                _idLookup[id].CleanReferencesExplicitlyOf(customBehaviour);
            }
        }

        public void CleanReferencesExplicitlyOf(CustomBehaviour behaviour)
        {
            ComponentId id = behaviour.id;
            _typeLookUp.CleanReferencesExplicitlyOf(behaviour);
            _goLookup[id._goInsId].CleanReferencesExplicitlyOf(behaviour);

            int oid = id.objBt;
            if (oid != ComponentId.None)
                _idLookup[oid].CleanReferencesExplicitlyOf(behaviour);

            Type t = behaviour.GetType();
            if(_pool.ContainsKey(t))
                _pool[t].CleanSpecificRefOf(behaviour);

        }


        public void SetComponentId(CustomBehaviour behaviour, ComponentId newId)
        {
            ChangeIdRefFor(behaviour, newId);
        }
        public void LogIdLookup()
        {
        }
        public void LogLookup()
        {
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
                _idLookup[oldId].CleanReferencesExplicitlyOf(behaviour);
                behaviour.id = newId;
                CheckAndAddToIdLookup(behaviour);
            }
        }

        #endregion

        #region EXCEPTIONS

        bool DoesIdExist(int t, Dictionary<int, TypeLookUp> targetDict)
        {
            return targetDict.ContainsKey(t);
        }

        private void RootLibraryMissingExceptionCheck()
        {
            if (RootLibrary.Instance == null)
                throw new Exception("RootLibrary instance not found, either create one or check script execution order for Root Library to execute before scene library");
        }

        private void NullElementPoolExceptionCheck(CustomBehaviour behaviour)
        {
            if (behaviour == null)
                throw new Exception("Trying to pool null element is not allowed!");
        }

       
        void MissingIdExceptionCheck(int t)
        {
            if (!_idLookup.ContainsKey(t))
                Debug.Log($"No component of id {t} present in lookup, Make sure to scan library once or instantiate via CustomBehaviour");
        }

        bool MissingPoolExceptionCheck(Type t) 
        {
            return (!_pool.ContainsKey(t) || _pool[t].ObjectCount <= 0);
                
        }
        #endregion


        public void SetBehaviours(CustomBehaviour[] behv)
        {
            behaviours = behv;
        }

        public void LogGOLookup()
        {
            throw new NotImplementedException();
        }

        

        public enum ExecutionMode 
        { 
            MANAGED_REFERENCES,
            UNMANAGED_REFERENCES
        }

    }


    public class PerformancePool<T> where T: CustomBehaviour
    {
        private List<T> objects = new List<T>();

        public int ObjectCount { get { return objects.Count; } }
        
        public void Pool(T behaviour)
        {
            objects.Add(behaviour);
        }

        public T GetPooled()
        {
            int objectCount = objects.Count;
            objectCount--;

            T ob = objects[objectCount];
            objects.RemoveAt(objectCount);
            return ob;
        }

        public void CleanNullReferences() 
        {
            for (int i = 0; i < objects.Count;)
            {
                if (objects[i]==null)
                {
                    objects.RemoveAt(i);
                    continue;
                }

                i++;
            }
        }

        public void CleanSpecificRefOf(CustomBehaviour behaviour) 
        {

            for (int i = 0; i < objects.Count;)
            {
                if (objects[i] == behaviour)
                {
                    objects.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }
    
    }

}
