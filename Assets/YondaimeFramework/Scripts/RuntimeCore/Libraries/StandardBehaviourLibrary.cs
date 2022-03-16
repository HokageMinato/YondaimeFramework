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
        private Dictionary<Type, List<CustomBehaviour>> _behaviourLookup;
        private Dictionary<int, List<CustomBehaviour>> _idLookup;
        RootLibrary _rootLibrary;

        public SceneId SceneId => sceneId;
        #endregion

        #region INITIALIZERS

        public void Awake()
        {
            _rootLibrary = RootLibrary.Instance;
            RootLibraryExistanceCheck();
            RegisterSelfInRootLibrary();
            GenerateBehaviourLookups();
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
                    behaviours[i].RefreshIds();
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
        
        #endregion

        #region COMPONENT_GETTERS

        public T GetBehaviourFromLibrary<T>()
        {
            Type reqeuestedType = typeof(T);
            MissingTypeExceptionCheck(reqeuestedType);

            return (T)(object)_behaviourLookup[reqeuestedType][0];
        }

        public  T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId)
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

        public  T GetBehaviourFromLibraryById<T>(int behaviourId)
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

            List<T> returnList= new List<T>(objectCount);

            for (int i = 0; i < objectCount; i++)
            {
                CustomBehaviour behaviour = behavioursInLookUp[i];
                
                if(behaviour.id._goInsId == requesteeGameObjectInstanceId)
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

        #endregion

        #region ALLOCATOR_HANDLES

        public void AddBehaviour<T>(T newBehaviour)
        {
            // LogLookup();

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

            if(!_idLookup.ContainsKey(id))   
                _idLookup.Add(id, new List<CustomBehaviour>());

            _idLookup[id].Add(newBehaviour);
        }

        public void CleanNullReferencesFor<T>(int id) 
        {
            Type t = typeof(T);

             CleanBehaviourLibReferencesOf(t);

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++) 
            {
                CleanBehaviourLibReferencesOf(itypes[i]);
            }

            if(id != ComponentId.None)
                CleanIdLibReferencesFor(id);
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
            if (!_behaviourLookup.ContainsKey(t))
                return; 

               List<CustomBehaviour> behaviours = _behaviourLookup[t];

                for (int i = 0; i < behaviours.Count;)
                {
                    if (behaviours[i]==null)
                    {
                        behaviours.RemoveAt(i);
                        continue;
                    }
                    
                    i++;
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

        void MissingTypeExceptionCheck(Type t) 
        {
            if (!_behaviourLookup.ContainsKey(t))
                throw new Exception($"No component of type {t} present in lookup, Make sure to scan library once or instantiate via CustomBehaviour");
        }
        
        void MissingIdExceptionCheck(int t) 
        {
            if (!_idLookup.ContainsKey(t) || _idLookup[t].Count <=0)
                throw new Exception($"No component of id {t} present in lookup, Make sure to scan library once or instantiate via CustomBehaviour");
        }

        #endregion

    }
}
