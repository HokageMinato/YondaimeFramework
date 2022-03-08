using System;
using System.Collections.Generic;
using UnityEngine;


 namespace YondaimeFramework
{

    
    public abstract class BehaviourLibraryOld : CustomBehaviour
    {

        #region PRIVATE_VARIABLES
        //Serialized
        [SerializeField] protected List<CustomBehaviour> _behaviours;
        [SerializeField] protected BehaviourLibraryOld[] _childLibs;

        public abstract void InitializeLibrary();
        public abstract T GetBehaviourFromLibrary<T>();
        public abstract T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId);
        public abstract T GetBehaviourFromLibraryById<T>(int behaviourId);
        public abstract void GetBehavioursFromLibrary<T>(List<T> behaviourListToBeFilled);
        public abstract void GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId, List<T> behaviourListToBeFilled);
        

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

                List<BehaviourLibraryOld> tempLibs = new List<BehaviourLibraryOld>();
                for (int i = 0; i < _behaviours.Count; i++)
                {
                    if (_behaviours[i] is BehaviourLibraryOld && !_behaviours[i].Equals(this))
                    {
                        tempLibs.Add((BehaviourLibraryOld)_behaviours[i]);
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

                List<BehaviourLibraryOld> myLibs = new List<BehaviourLibraryOld>(_childLibs);

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
                    //_behaviours[i].SetLibrary(this);
                    //_behaviours[i].SetLibrary(_mySceneLibrary);
                    
                    //if(this is PooledBehaviourLibrary);
                    //    _behaviours[i].SetLibrary(this as PooledBehaviourLibrary);
                }
            }
        }

        public virtual void PreRedundantCheck() { }

        #endregion
    }
};