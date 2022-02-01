using System;
using System.Collections.Generic;
using UnityEngine;


 namespace YondaimeFramework
{
    public class BehaviourLibrary : CustomBehaviour
    {

        #region PRIVATE_VARIABLES
        //Serialized
        [SerializeField] protected List<CustomBehaviour> _behaviours;
        [SerializeField] protected BehaviourLibrary[] _childLibs;

        //NonSerialized
        private Dictionary<Type, CustomBehaviour[]> _behaviourLookUp = new Dictionary<Type, CustomBehaviour[]>();
        private Dictionary<string, CustomBehaviour> _idLookup = new Dictionary<string, CustomBehaviour>();
        #endregion

        

        #region PUBLIC_METHODS
        public void InitializeLibrary()
        {
            Dictionary<Type, List<CustomBehaviour>> tempLookUp = new Dictionary<Type, List<CustomBehaviour>>();
            Dictionary<string, CustomBehaviour> tempLookUpId = new Dictionary<string, CustomBehaviour>();
            GenerateTempLookUps();
            ParseTempToFinalLookup();
            InitChildLibraries();
            

            void GenerateTempLookUps()
            {
                for (int i = 0; i < _behaviours.Count; i++)
                {
                    CustomBehaviour currentBehaviour = _behaviours[i];
                    Type currentBehaviourType = currentBehaviour.GetType();
                    string currentBehaviourId = currentBehaviour.Id;
                    
                    if (!tempLookUp.ContainsKey(currentBehaviourType))
                    {
                        tempLookUp.Add(currentBehaviourType, new List<CustomBehaviour>());
                    }

                    tempLookUp[currentBehaviourType].Add(_behaviours[i]);

                    if (!IsCustomId(currentBehaviourId))
                        continue;

                    currentBehaviourId += currentBehaviourType.ToString();
                    if (!tempLookUpId.ContainsKey(currentBehaviourId)) 
                    {
                        tempLookUpId.Add(currentBehaviourId, currentBehaviour);
                    }
                }
            }
            void ParseTempToFinalLookup()
            {
                foreach (KeyValuePair<Type, List<CustomBehaviour>> item in tempLookUp)
                {
                    if (!_behaviourLookUp.ContainsKey(item.Key))
                        _behaviourLookUp.Add(item.Key, item.Value.ToArray());
                    else
                        _behaviourLookUp[item.Key] = item.Value.ToArray();
                }

                foreach (KeyValuePair<string, CustomBehaviour> item in tempLookUpId)
                {
                    if (!_idLookup.ContainsKey(item.Key))
                        _idLookup.Add(item.Key, item.Value);
                    else
                        _idLookup[item.Key] = item.Value;
                }

            }
            void InitChildLibraries()
            {
                for (int i = 0; i < _childLibs.Length; i++)
                {
                    _childLibs[i].InitializeLibrary();
                }
            }
        }



        protected T GetBehaviourFromLibrary<T>()
        {
            Type reqeuestedType = typeof(T);

            if (_behaviourLookUp.ContainsKey(reqeuestedType) && _behaviourLookUp[reqeuestedType].Length > 0)
                return (T)(object)_behaviourLookUp[reqeuestedType][0];

            
            if (reqeuestedType.IsInterface)
            {
                foreach (KeyValuePair<Type, CustomBehaviour[]> item in _behaviourLookUp)
                {
                    if (item.Value.Length > 0 &&
                        reqeuestedType.IsAssignableFrom(item.Key))
                    {
                        return (T)(object)item.Value[0];
                    }
                }
            }

            for (int i = 0; i < _childLibs.Length; i++)
            {
                T behaviour =_childLibs[i].GetBehaviourFromLibrary<T>();
                if (behaviour != null)
                    return behaviour;
            }

            return default;
        }

        protected T GetBehaviourOfGameObject<T>(GameObject requesteeObject) 
        {
            Type reqeuestedType = typeof(T);

            if (_behaviourLookUp.ContainsKey(reqeuestedType) && _behaviourLookUp[reqeuestedType].Length > 0)
            {
                CustomBehaviour[] behaviours = _behaviourLookUp[reqeuestedType];

                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (behaviours[i].gameObject.Equals(requesteeObject)) {
                        return (T)(object)behaviours[i];
                    }
                }
            }


            if (reqeuestedType.IsInterface)
            {
                foreach (KeyValuePair<Type, CustomBehaviour[]> item in _behaviourLookUp)
                {
                    if (item.Value.Length > 0 && reqeuestedType.IsAssignableFrom(item.Key))
                    {
                        CustomBehaviour[] behaviours = item.Value;
                        for (int i = 0; i < behaviours.Length; i++)
                        {
                            if (behaviours[i].gameObject.Equals(requesteeObject))
                            {
                                return (T)(object)behaviours[i];
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < _childLibs.Length; i++)
            {
                T behaviour = _childLibs[i].GetBehaviourOfGameObject<T>(requesteeObject);
                if (behaviour != null)
                    return behaviour;
            }

            return default;

        }

        protected T GetBehaviourFromLibraryById<T>(string behaviourId) 
        {
            string id = behaviourId + typeof(T).ToString();

            if (_idLookup.ContainsKey(id))
                return (T)(object)_idLookup[id];

            for (int i = 0; i < _childLibs.Length; i++)
            {
                T behaviour = _childLibs[i].GetBehaviourFromLibraryById<T>(behaviourId);
                if (behaviour != null)
                    return behaviour;
            }

            return default;
        }

        protected List<T> GetBehavioursFromLibrary<T>()
        {
            Type reqeuestedType = typeof(T);
            List<T> behaviours = new List<T>();

            if (_behaviourLookUp.ContainsKey(reqeuestedType))
            {
                CustomBehaviour[] behavioursInLookUp = _behaviourLookUp[reqeuestedType];
                for (int i = 0; i < behavioursInLookUp.Length; i++)
                {
                    behaviours.Add((T)(object)behavioursInLookUp[i]);
                }
            }

            if (reqeuestedType.IsInterface)
            {

                foreach (KeyValuePair<Type, CustomBehaviour[]> item in _behaviourLookUp)
                {
                    if (reqeuestedType.IsAssignableFrom(item.Key))
                    {
                        CustomBehaviour[] behavioursInLookUp = item.Value;
                        for (int i = 0; i < behavioursInLookUp.Length; i++)
                        {
                            behaviours.Add((T)(object)behavioursInLookUp[i]);
                        }
                    }
                }
            }


            if (_childLibs.Length > 0)
            {
                for (int i = 0; i < _childLibs.Length; i++)
                {

                    behaviours.AddRange(_childLibs[i].GetBehavioursFromLibrary<T>());
                }
            }

            return behaviours;
        }

        protected List<T> GetBehavioursOfGameObject<T>(GameObject requesteeGameObject)
        {
            Type reqeuestedType = typeof(T);
            List<T> behaviours = new List<T>();

            if (_behaviourLookUp.ContainsKey(reqeuestedType))
            {
                CustomBehaviour[] behavioursInLookUp = _behaviourLookUp[reqeuestedType];
                for (int i = 0; i < behavioursInLookUp.Length && behavioursInLookUp[i].gameObject.Equals(requesteeGameObject); i++)
                {
                    behaviours.Add((T)(object)behavioursInLookUp[i]);
                }
            }

            if (reqeuestedType.IsInterface)
            {

                foreach (KeyValuePair<Type, CustomBehaviour[]> item in _behaviourLookUp)
                {
                    if (reqeuestedType.IsAssignableFrom(item.Key))
                    {
                        CustomBehaviour[] behavioursInLookUp = item.Value;
                        for (int i = 0; i < behavioursInLookUp.Length && behavioursInLookUp[i].gameObject.Equals(requesteeGameObject); i++)
                        {
                            behaviours.Add((T)(object)behavioursInLookUp[i]);
                        }
                    }
                }
            }


            if (_childLibs.Length > 0)
            {
                for (int i = 0; i < _childLibs.Length; i++)
                {
                    behaviours.AddRange(_childLibs[i].GetBehavioursFromLibrary<T>());
                }
            }

            return behaviours;
        }



        
        public void LogLibrary() {
            string items = "";
            foreach (var item in _behaviours)
            {
                items+=item.GetType()+"\n";

            }
            FrameworkLogger.Log(items);
        }

        public virtual void ScanBehaviours()
        {
            _behaviours = new List<CustomBehaviour>(GetComponentsInChildren<CustomBehaviour>(true));
            PreRedundantCheck();
            ScanChildLibs();
            RemoveRedundantBehavioursRecursive();
            RemoveRedundantChildLibRecursive();
            SetActiveLibraries();


            void ScanChildLibs()
            {

                List<BehaviourLibrary> tempLibs = new List<BehaviourLibrary>();
                for (int i = 0; i < _behaviours.Count; i++)
                {
                    if (_behaviours[i] is BehaviourLibrary && !_behaviours[i].Equals(this))
                    {
                        tempLibs.Add((BehaviourLibrary)_behaviours[i]);
                    }
                }
                _childLibs = tempLibs.ToArray();


                for (int i = 0; i < _childLibs.Length; i++)
                {
                    _childLibs[i].ScanBehaviours();
                }

            }
            void RemoveRedundantChildLibRecursive()
            {

                List<BehaviourLibrary> myLibs = new List<BehaviourLibrary>(_childLibs);

                for (int i = 0; i < _childLibs.Length; i++)
                {
                    for (int j = 0; j < _childLibs[i]._childLibs.Length; j++)
                    {
                        myLibs.Remove(_childLibs[i]._childLibs[j]);
                        _childLibs[i].ScanBehaviours();
                    }
                }

                _childLibs = myLibs.ToArray();
            }
            void RemoveRedundantBehavioursRecursive()
            {

                for (int i = 0; i < _childLibs.Length; i++)
                {
                    for (int j = 0; j < _childLibs[i]._behaviours.Count; j++)
                    {
                        _behaviours.Remove(_childLibs[i]._behaviours[j]);
                    }
                }


            }
            void SetActiveLibraries()
            {
                for (int i = 0; i < _behaviours.Count; i++)
                {
                    _behaviours[i].SetLibrary(this);
                    _behaviours[i].SetLibrary(MySystemLibrary);
                }
            }
        }

        public virtual void PreRedundantCheck() { }

        private bool IsCustomId(string behaviourId) 
        {
            return (!string.IsNullOrEmpty(behaviourId) &&
                            !string.IsNullOrWhiteSpace(behaviourId)) ;
        }
        #endregion
    }
};