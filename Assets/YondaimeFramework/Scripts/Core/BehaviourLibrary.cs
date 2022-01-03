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
        [SerializeField] public SystemId systemId;
        [SerializeField] private List<CustomBehaviour> _behaviours;
        [SerializeField] private BehaviourLibrary[] _childLibs;
        [SerializeField] private bool IsSystemRoot;

        //NonSerialized
        private Dictionary<Type, CustomBehaviour[]> _behaviourLookUp = new Dictionary<Type, CustomBehaviour[]>();

        //Constatns
        private const string SCAN_METHOD = "Scan";
        #endregion

        private void Start()
        {
            InitializeLookUp();
        }

        #region PUBLIC_METHODS
        public void InitializeLookUp()
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
                    _childLibs[i].InitializeLookUp();
                }
            }
        }

        public void SetSystemId(SystemId parentSystemId) {
            systemId = parentSystemId;
        }

        public T GetBehaviourFromLibrary<T>()
        {
            List<T> b= GetBehavioursFromLibrary<T>();
            if (b.Count > 0)
                return b[0];
            else
                return default;

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
                    if (typeof(T).IsAssignableFrom(item.Key))
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



        [ContextMenu(SCAN_METHOD)]
        public void ScanBehaviours()
        {
            _behaviours = new List<CustomBehaviour>(GetComponentsInChildren<CustomBehaviour>(true));
            SetSystemLibrary();
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

                if (IsSystemRoot) {
                    for (int i = 0; i < _childLibs.Length; i++)
                    {
                        _childLibs[i].SetSystemId(systemId);
                    }
                }

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
            void SetSystemLibrary() {
                if (IsSystemRoot)
                {
                    for (int i = 0; i < _behaviours.Count; i++)
                    {
                        _behaviours[i].SetSystemLibrary(this);
                    }
                }
            }
        }


        

        [ContextMenu(SCAN_METHOD,true)]
        public bool ScanTypeValidation() {
            return IsSystemRoot;
        }
        #endregion
    }
}