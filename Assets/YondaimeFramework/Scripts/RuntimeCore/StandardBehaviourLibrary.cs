using System;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    public class StandardBehaviourLibrary 
    {

        #region LOOKUPS
        private Dictionary<Type, List<CustomBehaviour>> _behaviourLookup;
        private Dictionary<int, List<CustomBehaviour>> _idLookup;
        #endregion

        #region INITIALIZERS
        public void InitLibrary(Dictionary<Type, List<CustomBehaviour>> behaviourLookup, 
                                Dictionary<int, List<CustomBehaviour>> idLookup) 
        {
            _behaviourLookup = behaviourLookup;
            _idLookup = idLookup;   
        }

        

        internal void LogLookup()
        {
            string val = string.Empty;
            foreach (KeyValuePair<Type, List<CustomBehaviour>> item in _behaviourLookup)
            {
                string v = $"Type {item.Key}, TotalInstances {item.Value.Count}  GOS=>[";
                for (int i = 0; i < item.Value.Count; i++)
                {
                    if (item.Value[i] == null)
                        Debug.LogError($"Null at lookup for{item.Key}");

                    v += $" {item.Value[i].gameObject.name} . \n"; 
                }
                val += $" {v} ] \n\n\n";
            }
            Debug.Log(val);
        }
        #endregion

        #region COMPONENT_GETTERS

        public T GetBehaviourFromLibrary<T>()
        {
            Type reqeuestedType = typeof(T);
            return (T)(object)_behaviourLookup[reqeuestedType][0];
        }

        public  T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
            Type reqeuestedType = typeof(T);

            List<CustomBehaviour> behaviours = _behaviourLookup[reqeuestedType];
            int total = behaviours.Count;

            for (int i = 0; i < total; i++)
            {
                 if (behaviours[i].GOInstanceId == requesteeGameObjectInstanceId)
                 {
                     return (T)(object)behaviours[i];
                 }
            }
            
            return default;
        }

        public  T GetBehaviourFromLibraryById<T>(int behaviourId)
        {
            List<CustomBehaviour> behv = _idLookup[behaviourId];
            int count = behv.Count;

            for (int i = 0; i < count; i++)
            {
                    if (behv[i] is T)
                        return (T)(object)behv[i];
            }

            return default;
        }

        public List<T> GetBehavioursFromLibrary<T>()
        {
            Type reqeuestedType = typeof(T);

            List<CustomBehaviour> behavioursInLookUp = _behaviourLookup[reqeuestedType];
            int totalObjectCount = behavioursInLookUp.Count;

            List<T> returnList = new List<T>(totalObjectCount);

            for (int i = 0; i < totalObjectCount; i++)
            {
                returnList.Add((T)(object)behavioursInLookUp[i]);
            }

            return returnList;
        }

        public List<T> GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
            Type reqeuestedType = typeof(T);

            List<CustomBehaviour> behavioursInLookUp = _behaviourLookup[reqeuestedType];
            int objectCount = behavioursInLookUp.Count;

            List<T> returnList= new List<T>(objectCount);
            for (int i = 0; i < objectCount && behavioursInLookUp[i].GOInstanceId == requesteeGameObjectInstanceId; i++)
            {
                returnList.Add((T)(object)behavioursInLookUp[i]);
            }

            return returnList;
        }


        #endregion

        #region ALLOCATOR_HANDLES

        public void AddBehaviour<T>(T newBehaviour)
        {
            LogLookup();

            Type t = typeof(T);
            if (_behaviourLookup.ContainsKey(t))
            {
                AppendBehaviour(newBehaviour, t);
            }
            else
            {
                _behaviourLookup.Add(t, new List<CustomBehaviour>() { (CustomBehaviour)(object)newBehaviour });
            }


            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++)
            {
                t = itypes[i];
                if (_behaviourLookup.ContainsKey(t))
                {
                    AppendBehaviour(newBehaviour, t);
                }
                else
                {
                    _behaviourLookup.Add(t, new List<CustomBehaviour>() { (CustomBehaviour)(object)newBehaviour });
                }
            }

            LogLookup();
        }

        public void AddBehaviours<T>(List<T> customBehaviour) 
        {
            Type t = typeof(T);

            if (_behaviourLookup.ContainsKey(t))
            {
                AppendBehavioursListInternal(customBehaviour, t);
            }
            else
            {
                AddListInternal(customBehaviour, t);
            }

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++)
            {
                t = itypes[i];  
                if (_behaviourLookup.ContainsKey(t))
                {
                    AppendBehavioursListInternal(customBehaviour, t);
                }
                else
                {
                    AddListInternal(customBehaviour, t);
                }
            }


        }

        public void AddBehaviours<T>(T[] customBehaviour) 
        {
            Type t = typeof(T);
            
            if (_behaviourLookup.ContainsKey(t))
            {
                AppendBehavioursArrayInternal(customBehaviour, t);
            }
            else
            {
                AddArrayInternal(customBehaviour, t);
            }


            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++)
            {
                t = itypes[i];
                if (_behaviourLookup.ContainsKey(t))
                {
                    AppendBehavioursArrayInternal(customBehaviour,t);
                }
                else
                {
                    AddArrayInternal<T>(customBehaviour, t);
                }
            }

        }

        public void CleanReferencesFor(CustomBehaviour customBehaviour) 
        {
            LogLookup();
            Type t = customBehaviour.GetType();

             CleanReferencesFor(t);

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++) 
            { 
                CleanReferencesFor(itypes[i]);
            }
            LogLookup();
        }

        #endregion

        #region INTERNAL_ALLOCATION_WORKERS

        private void AppendBehaviour<T>(T newBehaviour, Type t)
        {
            _behaviourLookup[t].Add((CustomBehaviour)(object)newBehaviour); ;
        }

        private void CleanReferencesFor(Type t)
        {

            if (_behaviourLookup.ContainsKey(t))
            {
               List<CustomBehaviour> behaviours = _behaviourLookup[t];

                for (int i = 0; i < behaviours.Count; i++)
                {
                    if(behaviours[i]==null)
                        behaviours.RemoveAt(i);
                }
              
            }
        }

        private void AppendBehavioursListInternal<T>(List<T> customBehaviours, Type t)
        {
            List<CustomBehaviour> oldComponents = _behaviourLookup[t];
            oldComponents.Capacity += customBehaviours.Count;

            for (int j = 0; j < customBehaviours.Count; j++)
            {
                oldComponents.Add((CustomBehaviour)(object)customBehaviours[j]);
            }
        }

        private void AppendBehavioursArrayInternal<T>(T[] customBehaviours, Type t)
        {
            List<CustomBehaviour> oldComponents = _behaviourLookup[t];
            oldComponents.Capacity += customBehaviours.Length;

            for (int j = 0; j < customBehaviours.Length; j++)
            {
                oldComponents.Add((CustomBehaviour)(object)customBehaviours[j]);
            }
        }

        private void AddListInternal<T>(List<T> customBehaviour, Type t)
        {
           List<CustomBehaviour> components = new List<CustomBehaviour>(customBehaviour.Count);
            
            for (int i = 0; i < customBehaviour.Count; i++)
            {
                components[i] = (CustomBehaviour)(object)customBehaviour[i];
            }

            _behaviourLookup.Add(t, components);
        }
        
        private void AddArrayInternal<T>(T[] customBehaviour, Type t)
        {
            List<CustomBehaviour> components = new List<CustomBehaviour>(customBehaviour.Length);

            for (int i = 0; i < customBehaviour.Length; i++)
            {
                components[i] = (CustomBehaviour)(object)customBehaviour[i];
            }

            _behaviourLookup.Add(t, components);
        }
        


        #endregion

    }
}
