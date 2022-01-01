using System;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    public abstract class BaseBehaviourLibrary : CustomBehaviour
    {
        #region PUBLIC_VARIABLE
        #endregion

        #region PRIVATE_VARIABLES
        [SerializeField] private CustomBehaviour[] _behaviours;
        [SerializeField] private BaseBehaviourLibrary[] _childLibs;
        private Dictionary<Type, List<CustomBehaviour>> _behaviourLookUp = new Dictionary<Type, List<CustomBehaviour>>();
        #endregion

        #region PUBLIC_METHODS
        public void InitializeLookUp()
        {
            _behaviourLookUp.Clear();
            GenerateLookUp();
            FillLookUp();
            InitChildLibraries();

            void GenerateLookUp()
            {
                for (int i = 0; i < _behaviours.Length; i++)
                {
                    Type behaviourType = _behaviours[i].GetType();
                    if (!_behaviourLookUp.ContainsKey(behaviourType))
                    {
                        _behaviourLookUp.Add(behaviourType, new List<CustomBehaviour>());
                    }
                }
            }
            void FillLookUp()
            {
                for (int i = 0; i < _behaviours.Length; i++)
                {
                    _behaviourLookUp[_behaviours[i].GetType()].Add(_behaviours[i]);
                }
            }
            void InitChildLibraries()
            {
                for (int i = 0; i < _childLibs.Length; i++)
                {
                    _childLibs[i].InitializeLookUp();
                }
            }
        }

        public List<T> GetBehavioursFromLibrary<T>()
        {
            Type reqeuestedType = typeof(T);
            List<T> behaviours = new List<T>();

            if (_behaviourLookUp.ContainsKey(reqeuestedType))
            {
                List<CustomBehaviour> behavioursInLookUp = _behaviourLookUp[reqeuestedType];
                for (int i = 0; i < behavioursInLookUp.Count; i++)
                {
                    behaviours.Add((T)(object)behavioursInLookUp[i]);
                }
            }

            if (reqeuestedType.IsInterface)
            {
                foreach (KeyValuePair<Type, List<CustomBehaviour>> item in _behaviourLookUp)
                {
                    if (typeof(T).IsAssignableFrom(item.Key))
                    {
                        List<CustomBehaviour> behavioursInLookUp = item.Value;
                        for (int i = 0; i < behavioursInLookUp.Count; i++)
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

        public List<T> GetBehavioursFromChildLibrary<T>()
        {

            List<T> behaviours = GetBehavioursFromLibrary<T>();

            for (int i = 0; i < _childLibs.Length; i++)
            {
                behaviours.AddRange(_childLibs[i].GetBehavioursFromChildLibrary<T>());
            }

            return behaviours;
        }

        public T GetBehaviourFromLibrary<T>()
        {
            return GetBehavioursFromLibrary<T>()[0];
        }

        public T GetBehaviourFromChildLibrary<T>()
        {
            return GetBehavioursFromChildLibrary<T>()[0];
        }




        [ContextMenu("Scan")]
        public void ScanTypes()
        {
            List<CustomBehaviour> scannedBehaviour = new List<CustomBehaviour>(GetComponentsInChildren<CustomBehaviour>(true));
            List<BehaviourLibrary> childLibraries = new List<BehaviourLibrary>();

            

            RemoveRedundantBehaviours();
            AssignScannedBehaviours();
            SetSelfAsActiveLibraryToBehaviours();
            _childLibs = childLibraries.ToArray();
            InitializeLookUp();

            Debug.LogWarning($"Library Refreshed, Make sure to decrese instantiation and destroy calls at {gameObject.name}");


            void RemoveRedundantBehaviours()
            {
                for (int i = 0; i < scannedBehaviour.Count; i++)
                {
                    if (IsAChildLibrary(scannedBehaviour[i]))
                    {
                        BehaviourLibrary foundLibrary = scannedBehaviour[i] as BehaviourLibrary;
                        FilterScannedBehavioursByLibrary(foundLibrary);
                        AddToChildLibrares(foundLibrary);
                    }
                }
            }
            void SetSelfAsActiveLibraryToBehaviours()
            {
                for (int i = 0; i < _behaviours.Length; i++)
                {
                   // _behaviours[i].SetLibrary(this);
                }
            }
            void AssignScannedBehaviours()
            {
                _behaviours = scannedBehaviour.ToArray();
            }
            bool IsAChildLibrary(CustomBehaviour behaviour)
            {
                return behaviour.GetType().Equals(typeof(BehaviourLibrary)) &&
                      !behaviour.Equals(this);
            }
            void FilterScannedBehavioursByLibrary(BehaviourLibrary library)
            {

              //  for (int i = 0; i < library._behaviours.Length; i++)
                {
                //    scannedBehaviour.Remove(library._behaviours[i]);
                }
            }
            void AddToChildLibrares(BehaviourLibrary library)
            {
              //  library.SetLibrary(this);
                childLibraries.Add(library);
            }

        }
        #endregion

    }




}
