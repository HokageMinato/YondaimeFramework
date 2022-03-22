using System.Collections.Generic;
using System;
using UnityEngine;

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
        private Dictionary<Type, List<CustomBehaviour>> _behaviourLookup;
        private Dictionary<int, List<CustomBehaviour>> _idLookup;
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
            GenerateBehaviourLookups();
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

        internal void LogPool() 
        { 
        
        }

        private void GenerateBehaviourLookups()
        {

            Dictionary<Type, List<CustomBehaviour>> behaviourLookup = new Dictionary<Type, List<CustomBehaviour>>();
            Dictionary<int, List<CustomBehaviour>> idLookup = new Dictionary<int, List<CustomBehaviour>>();
            CheckForEmptyBehaviours();
            GenerateBehaviourLookUp();
            _behaviourLookup = behaviourLookup;
            _idLookup = idLookup;



            void CheckForEmptyBehaviours()
            {
                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (behaviours[i] == null)
                        throw new Exception("Null object detected,Make sure to Scan library after making all scene edits");

                    behaviours[i].SetLibrary(this);
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
                if (!behaviourLookup.ContainsKey(t))
                {
                    behaviourLookup.Add(t, new List<CustomBehaviour>() { behaviour });
                    return;
                }

                behaviourLookup[t].Add(behaviour);
            }
            void AddToIdLookup(CustomBehaviour behaviour)
            {
                int id = behaviour.id.objBt;

                if (!idLookup.ContainsKey(id))
                {
                    idLookup.Add(id, new List<CustomBehaviour>() { behaviour });
                    return;
                }

                idLookup[id].Add(behaviour);
            }

        }

        private void SetExecutionMode() 
        {
            modeFlag = (executionMode == ExecutionMode.MANAGED_REFERENCES) ? Managed : UnManaged; 
        }

        #endregion

        #region COMPONENT_GETTERS

        public T GetBehaviourFromLibrary<T>()
        {
            Type reqeuestedType = typeof(T);

            MissingTypeExceptionCheck(reqeuestedType);

            return (T)(object)_behaviourLookup[reqeuestedType][0];
        }

        public T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
            Type reqeuestedType = typeof(T);

            MissingTypeExceptionCheck(reqeuestedType);

            List<CustomBehaviour> behaviours = _behaviourLookup[reqeuestedType];
            int total = behaviours.Count;

            for (int i = 0; i < total; i++)
            {
                CustomBehaviour behaviour = behaviours[i];
                if (behaviour.id._goInsId == requesteeGameObjectInstanceId)
                    return (T)(object)behaviour;

            }

            return default;
        }

        public T GetBehaviourFromLibraryById<T>(int behaviourId)
        {
            MissingIdExceptionCheck(behaviourId);

            List<CustomBehaviour> behv = _idLookup[behaviourId];
            int count = behv.Count;

            for (int i = 0; i < count; i++)
            {
                CustomBehaviour behaviour = behv[i];
                if (behaviour is T)
                    return (T)(object)behaviour;
            }

            return default;
        }

        public List<T> GetBehavioursFromLibrary<T>()
        {
            Type reqeuestedType = typeof(T);

            MissingTypeExceptionCheck(reqeuestedType);

            List<CustomBehaviour> behavioursInLookUp = _behaviourLookup[reqeuestedType];
            int totalObjectCount = behavioursInLookUp.Count;

            List<T> returnList = new List<T>(totalObjectCount);
            for (int i = 0; i < totalObjectCount; i++)
                returnList.Add((T)(object)behavioursInLookUp[i]);



            return returnList;
        }

        public List<T> GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
            Type reqeuestedType = typeof(T);

            MissingTypeExceptionCheck(reqeuestedType);

            List<CustomBehaviour> behavioursInLookUp = _behaviourLookup[reqeuestedType];
            int objectCount = behavioursInLookUp.Count;

            List<T> returnList = new List<T>(objectCount);

            for (int i = 0; i < objectCount; i++)
            {
                CustomBehaviour behaviour = behavioursInLookUp[i];

                if (behaviour.id._goInsId == requesteeGameObjectInstanceId)
                    returnList.Add((T)(object)behaviour);
            }

            return returnList;
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

        public T GetPooled<T>()
        {
            Type type = typeof(T);

            MissingPoolExceptionCheck(type);

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
                CleanPooledStateReferencesFor(behaviour);

            behaviour.OnPooled();
        }


        #endregion

        #region ALLOCATOR_HANDLES

        public void AddBehaviour<T>(T newBehaviour)
        {
            Type t = typeof(T);
            CustomBehaviour behaviour = (CustomBehaviour)(object)newBehaviour;

            if (_behaviourLookup.ContainsKey(t))
            {
                AppendBehaviour(behaviour, t);
            }
            else
            {
                GenerateBehaviourTable(behaviour, t);
            }

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++)
            {
                t = itypes[i];
                if (_behaviourLookup.ContainsKey(t))
                {
                    AppendBehaviour(behaviour, t);
                }
                else
                {
                    GenerateBehaviourTable(behaviour, t);
                }
            }

            CheckAndAddToIdLookup(behaviour);
        }
        private void CheckAndAddToIdLookup(CustomBehaviour newBehaviour)
        {
            int id = newBehaviour.id.objBt;

            newBehaviour.RefreshIds();

            if (id == ComponentId.None)
                return;

            if (!_idLookup.ContainsKey(id))
                _idLookup.Add(id, new List<CustomBehaviour>());

            _idLookup[id].Add(newBehaviour);
        }
        public void CleanNullReferencesFor(ComponentId id,Type t)
        {
            int cid = id.objBt;

            CleanBehaviourLibReferencesOf(t);
            CleanBehaviourReferencesFromPoolOf(t);

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++)
            {
                CleanBehaviourLibReferencesOf(itypes[i]);
                CleanBehaviourReferencesFromPoolOf(t);
            }

            if(cid != ComponentId.None)
            CleanIdLibReferencesFor(cid);

            
        }
        public void CleanPooledStateReferencesFor(CustomBehaviour customBehaviour)
        {
            Type t = customBehaviour.GetType();

            CleanPooledStateBehaviourLibReferencesOf(t);

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++)
            {
                CleanPooledStateBehaviourLibReferencesOf(itypes[i]);
            }

            int id = customBehaviour.id.objBt;
            if (id!=ComponentId.None)
                CleanPooledStateIdLibReferencesFor(id);
        }

        public void SetComponentId(CustomBehaviour behaviour, ComponentId newId)
        {
            ChangeIdRefFor(behaviour, newId);
        }
        public void LogIdLookup()
        {
             LogLookup(_idLookup,"Idlookup");
        }
        public void LogLookup()
        {
             LogLookup(_behaviourLookup,"Behv Lookup");
        }


        #endregion

        #region INTERNAL_ALLOCATION_WORKERS

        private void GenerateBehaviourTable(CustomBehaviour newBehaviour, Type t)
        {
            _behaviourLookup.Add(t, new List<CustomBehaviour>() { newBehaviour });
        }
        private void AppendBehaviour(CustomBehaviour newBehaviour, Type t)
        {
            _behaviourLookup[t].Add(newBehaviour);

        }
        private void CleanIdLibReferencesFor(int id)
        {
            if (!_idLookup.ContainsKey(id))
                return;

            List<CustomBehaviour> items = _idLookup[id];
            for (int i = 0; i < items.Count;)
            {
                if (items[i] == null)
                    items.RemoveAt(i);
                else
                    i++;
            }
        }         
        private void CleanBehaviourLibReferencesOf(Type t)
        {
            if (_behaviourLookup.ContainsKey(t))
            {
                List<CustomBehaviour> behaviours = _behaviourLookup[t];

                for (int i = 0; i < behaviours.Count;)
                {
                    if (behaviours[i]== null)
                    {
                        behaviours.RemoveAt(i);
                        continue;
                    }

                    i++;
                }

            }
        }
        private void CleanPooledStateIdLibReferencesFor(int id)
        {
            List<CustomBehaviour> items = _idLookup[id];
            for (int i = 0; i < items.Count;)
            {
                if (items[i].poolState == Pooled)
                    items.RemoveAt(i);
                else
                    i++;
            }
        }         
        private void CleanPooledStateBehaviourLibReferencesOf(Type t)
        {
            if (_behaviourLookup.ContainsKey(t))
            {
                List<CustomBehaviour> behaviours = _behaviourLookup[t];

                for (int i = 0; i < behaviours.Count;)
                {
                    if ((behaviours[i].poolState == Pooled))
                    {
                        behaviours.RemoveAt(i);
                        continue;
                    }

                    i++;
                }

            }
        }
        private void CleanBehaviourReferencesFromPoolOf(Type t) 
        {
            if (modeFlag == UnManaged)
                return;

            if (_pool.ContainsKey(t))
            {
                _pool[t].CleanNullReferences();
            }
        }

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
                List<CustomBehaviour> behv = _idLookup[oldId];

                for (int i = 0; i < behv.Count;)
                {
                    if (behv[i] == behaviour)
                    {
                        behv.RemoveAt(i);
                        continue;
                    }

                    i++;
                }

                
                behaviour.id = newId;
                CheckAndAddToIdLookup(behaviour);
            }
        }

        #endregion

        #region EXCEPTIONS
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

       
        void MissingTypeExceptionCheck(Type t)
        {
            if (!_behaviourLookup.ContainsKey(t))
                throw new Exception($"No component of type {t} present in lookup, Make sure to scan library once or instantiate via CustomBehaviour");
        }

        void MissingIdExceptionCheck(int t)
        {
            if (!_idLookup.ContainsKey(t) || _idLookup[t].Count <= 0)
                throw new Exception($"No component of id {t} present in lookup, Make sure to scan library once or instantiate via CustomBehaviour");
        }

        void MissingPoolExceptionCheck(Type t) 
        { 
            if(!_pool.ContainsKey(t) || _pool[t].ObjectCount <=0)
                throw new Exception($"Pool of  type {t} is empty, Make sure you pool elements before de-pooling elements");
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


    public class PerformancePool<T> where T: UnityEngine.Object
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

            if (objectCount <= 0)
                return default;

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
    
    }

}
