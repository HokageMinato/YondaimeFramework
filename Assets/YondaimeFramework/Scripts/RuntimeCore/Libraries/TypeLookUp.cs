using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YondaimeFramework
{
    public class TypeLookUp 
    {

        #region LOOKUP
        private Dictionary<Type, IList> _behaviourLookup = new Dictionary<Type, IList>();
        #endregion


        #region INITIALIZER
        public void GenerateLookUp(CustomBehaviour[] behaviours) 
        {
            Dictionary<Type, IList> behaviourLookup = new Dictionary<Type, IList>();
            CheckForEmptyBehaviours();
            GenerateBehaviourLookUp();
            _behaviourLookup = behaviourLookup;

            Debug.Log(_behaviourLookup == null);

            void CheckForEmptyBehaviours()
            {
                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (behaviours[i] == null)
                        throw new Exception("Null object detected,Make sure to Scan library after making all scene edits");

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
                }
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
                    behaviourLookup.Add(t, GenerateListOfType(t));
                }

                behaviourLookup[t].Add(behaviour);
            }
        }
        #endregion


        #region GETTERS

        public T GetBehaviour<T>() 
        {
            Type reqeuestedType = typeof(T);
            if (NoObjectsPresentOfType(reqeuestedType))
                return default;

            return (T)_behaviourLookup[reqeuestedType][0];
        }


        public IReadOnlyList<T> GetBehavioursFromContainer<T>()
        {
            Type reqeuestedType = typeof(T);

            if (NoObjectsPresentOfType(reqeuestedType))
                return default;


            return (IReadOnlyList<T>)_behaviourLookup[reqeuestedType];
        }
       

        #endregion


        #region OBJECT_ALLOCATORS

        public void AddBehaviour(CustomBehaviour newBehaviour)
        {
            Type t = newBehaviour.GetType();


            if (_behaviourLookup.ContainsKey(t))
            {
                _behaviourLookup[t].Add(newBehaviour);
            }
            else
            {
                _behaviourLookup.Add(t, GenerateListOfType(t));
            }

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++)
            {
                t = itypes[i];
                if (_behaviourLookup.ContainsKey(t))
                {
                    _behaviourLookup[t].Add(newBehaviour);
                }
                else
                {
                    _behaviourLookup.Add(t, GenerateListOfType(t));
                }
            }

        }

        
        public void CleanNullReferencesFor(Type t)
        {
            CleanNullReferencesOf(t);

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++)
            {
                CleanNullReferencesOf(itypes[i]);
            }

        }

        private void CleanNullReferencesOf(Type t)
        {
            if (!_behaviourLookup.ContainsKey(t))
                return;

            IList behaviours = _behaviourLookup[t];


            for (int i = 0; i < behaviours.Count;)
            {
                if (((CustomBehaviour)behaviours[i]) == null)
                {
                    behaviours.RemoveAt(i);
                    continue;
                }

                i++;
            }
        }

        public void CleanReferencesExplicitlyOf(CustomBehaviour behaviour,Type t) 
        {
            CleanReferenceExplicitlyOf(behaviour,t);

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++)
            {
                CleanReferenceExplicitlyOf(behaviour,itypes[i]);
            }

        }

        private void CleanReferenceExplicitlyOf(CustomBehaviour behaviour,Type t) 
        {
            if (!_behaviourLookup.ContainsKey(t))
                return;

            IList behaviours = _behaviourLookup[t];

            for (int i = 0; i < behaviours.Count;)
            {
                if (((CustomBehaviour)behaviours[i]) == behaviour)
                {
                    behaviours.RemoveAt(i);
                    continue;
                }

                i++;
            }
        }


        private IList GenerateListOfType(Type t)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(t);

            return (IList)Activator.CreateInstance(constructedListType);
        }
        #endregion


        #region EXCEPTIONS
        bool NoObjectsPresentOfType(Type t)
        {
            return (!_behaviourLookup.ContainsKey(t) || _behaviourLookup[t].Count <= 0);
            //if (!_behaviourLookup.ContainsKey(t) || _behaviourLookup[t].Count <= 0)
                //throw new Exception($"No component of type {t} present in lookup, Make sure to scan library once or instantiate via CustomBehaviour");
        }

        #endregion


        #region DEBUG_ACCESSORS
#if UNITY_EDITOR
        public Dictionary<Type, IList> lookup => _behaviourLookup;
        #endif
        #endregion


    }
}