using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YondaimeFramework
{
    public class TypeLookUp : MonoBehaviour
    {

        #region LOOKUP
        private Dictionary<Type, List<CustomBehaviour>> _behaviourLookup = new Dictionary<Type, List<CustomBehaviour>>();
        #endregion


        #region INITIALIZER
        public void GenerateLookUp(CustomBehaviour[] behaviours) 
        {
            Dictionary<Type, List<CustomBehaviour>> behaviourLookup = new Dictionary<Type, List<CustomBehaviour>>();
            CheckForEmptyBehaviours();
            GenerateBehaviourLookUp();
            _behaviourLookup = behaviourLookup;

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
                    behaviourLookup.Add(t, new List<CustomBehaviour>() { behaviour });
                    return;
                }

                behaviourLookup[t].Add(behaviour);
            }
        }
        #endregion


        #region GETTERS

        public T GetBehaviour<T>() 
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
        #endregion


        #region OBJECT_ALLOCATORS

        public void AddBehaviour<T>(T newBehaviour)
        {
            // LogLookup();

            Type t = typeof(T);

            CustomBehaviour behaviour = (CustomBehaviour)(object)newBehaviour;

            if (_behaviourLookup.ContainsKey(t))
            {
                _behaviourLookup[t].Add(behaviour);
            }
            else
            {
                _behaviourLookup.Add(t, new List<CustomBehaviour>() { behaviour });
            }

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++)
            {
                t = itypes[i];
                if (_behaviourLookup.ContainsKey(t))
                {
                    _behaviourLookup[t].Add(behaviour);
                }
                else
                {
                    _behaviourLookup.Add(t, new List<CustomBehaviour>() { behaviour });
                }
            }

        }

        public void CleanNullReferencesFor<T>()
        {
            Type t = typeof(T);
            CleanNullReferencesFor(t);
        }
        
        public void CleanNullReferencesFor(Type t)
        {
            CleanBehaviourLibReferencesOf(t);

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++)
            {
                CleanBehaviourLibReferencesOf(itypes[i]);
            }

        }

        private void CleanBehaviourLibReferencesOf(Type t)
        {
            if (!_behaviourLookup.ContainsKey(t))
                return;

            List<CustomBehaviour> behaviours = _behaviourLookup[t];

            for (int i = 0; i < behaviours.Count;)
            {
                if (behaviours[i] == null)
                {
                    behaviours.RemoveAt(i);
                    continue;
                }

                i++;
            }
        }
        #endregion


        #region EXCEPTIONS
        void MissingTypeExceptionCheck(Type t)
        {
            if (!_behaviourLookup.ContainsKey(t) || _behaviourLookup[t].Count <= 0)
                throw new Exception($"No component of type {t} present in lookup, Make sure to scan library once or instantiate via CustomBehaviour");
        }
        #endregion


        #region DEBUG_ACCESSORS
        #if UNITY_EDITOR
        public Dictionary<Type, List<CustomBehaviour>> lookup => _behaviourLookup;
        #endif
        #endregion


    }
}