using System;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    [System.Serializable]
    public struct SystemId {
        public string id;
    }

    public class BehaviourLibrary : CustomBehaviour
    {

        #region PRIVATE_VARIABLES
        //Serialized
        [SerializeField] private List<CustomBehaviour> _behaviours;
        [SerializeField] protected BehaviourLibrary[] _childLibs;
        
        //NonSerialized
        private Dictionary<Type, CustomBehaviour[]> _behaviourLookUp = new Dictionary<Type, CustomBehaviour[]>();
        #endregion

        

        #region PUBLIC_METHODS
        public void InitializeLibrary()
        {
            Dictionary<Type, List<CustomBehaviour>> tempLookUp = new Dictionary<Type, List<CustomBehaviour>>();

            GenerateTempLookUp();
            FillTempLookUp();
            ParseTempToFinalLookup();
            InitChildLibraries();

            void GenerateTempLookUp()
            {

                for (int i = 0; i < _behaviours.Count; i++)
                {
                    Type behaviourType = _behaviours[i].GetType();
                    if (!tempLookUp.ContainsKey(behaviourType))
                    {
                        tempLookUp.Add(behaviourType,new List<CustomBehaviour>());
                    }
                }
            }
            void FillTempLookUp()
            {
                for (int i = 0; i < _behaviours.Count; i++)
                {
                    tempLookUp[_behaviours[i].GetType()].Add(_behaviours[i]);
                }
            }
            void ParseTempToFinalLookup()
            {
                foreach (KeyValuePair<Type, List<CustomBehaviour>> item in tempLookUp)
                {
                    if (_behaviourLookUp.ContainsKey(item.Key))
                    {
                        int itemsLength = item.Value.Count;
                        for (int i = 0; i < itemsLength; i++)
                        {
                            _behaviourLookUp[item.Key][i] = item.Value[i];
                        }
                    }
                    else {
                        _behaviourLookUp.Add(item.Key, new CustomBehaviour[item.Value.Count]);
                    }
                }

                if (!FrameworkConstants.IsDebug)
                    _behaviours = null;

            }
            void InitChildLibraries()
            {
                for (int i = 0; i < _childLibs.Length; i++)
                {
                    _childLibs[i].InitializeLibrary();
                }
            }
        }
      
        public T GetBehaviourFromLibrary<T>()
        {
            T behaviour = default;
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
                behaviour = _childLibs[i].GetBehaviourFromLibrary<T>();
            }

            return behaviour;
        }

        public List<T> GetBehavioursFromLibrary<T>()
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



        
        public virtual void ScanBehaviours()
        {
            _behaviours = new List<CustomBehaviour>(GetComponentsInChildren<CustomBehaviour>(true));
            ScanChildLibs();
            RemoveRedundantBehavioursRecursive();
            RemoveRedundantChildLibRecursive();
            SetSelfAsActiveLibrary();
           

            void ScanChildLibs() {
                
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
            void RemoveRedundantChildLibRecursive() {

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
            void RemoveRedundantBehavioursRecursive() {

                for (int i = 0; i < _childLibs.Length; i++)
                {
                    for (int j = 0; j < _childLibs[i]._behaviours.Count; j++)
                    {
                        _behaviours.Remove(_childLibs[i]._behaviours[j]);
                    }
                }


            }
            void SetSelfAsActiveLibrary()
            {
                for (int i = 0; i < _behaviours.Count; i++)
                {
                     _behaviours[i].SetLibrary(this);
                }
            }
            
        }

        
        #endregion
    }
}