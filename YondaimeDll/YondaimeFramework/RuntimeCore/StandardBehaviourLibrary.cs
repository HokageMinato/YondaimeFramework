using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    public class StandardBehaviourLibrary 
    {

        #region LOOKUPS
        private Dictionary<Type, List<CustomBehaviour>> _behaviourLookup;
        private Dictionary<int, List<CustomBehaviour>> _idLookup;
        private Dictionary<Type, ArrayList> _testlkp;
        #endregion

        #region INITIALIZERS
        public void InitLibrary(Dictionary<Type, List<CustomBehaviour>> behaviourLookup, 
                                Dictionary<int, List<CustomBehaviour>> idLookup) 
        {
            _behaviourLookup = behaviourLookup;
            _idLookup = idLookup;   
        }

        

        internal void LogLookup<T,K>(Dictionary<T,K> dict,string name) where K: List<CustomBehaviour>
        {
            

            string val = $"Showinggg => {name}<=  ";
            foreach (KeyValuePair<T, K> item in dict)
            {
                string v = $"Type {item.Key}, TotalInstances {item.Value.Count}  GOS=>[";
                for (int i = 0; i < item.Value.Count; i++)
                {
                    if(item.Value[i]!=null)
                    v += $" {item.Value[i].gameObject.name} -- {item.Value[i].id.objBt} --- {item.Value[i].GetInstanceID()}. \n"; 
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

            if (!_behaviourLookup.ContainsKey(reqeuestedType))
                return default;

            return (T)(object)_behaviourLookup[reqeuestedType][0];
        }

        public  T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
            Type reqeuestedType = typeof(T);

            if (!_behaviourLookup.ContainsKey(reqeuestedType))
                return default;

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
            if(!_idLookup.ContainsKey(behaviourId))
                return default;

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
                returnList.Add((T)(object)behavioursInLookUp[i]);


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
            Type t = customBehaviour.GetType();

             CleanBehaviourLibReferencesOf(t);

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++) 
            {
                CleanBehaviourLibReferencesOf(itypes[i]);
            }

            CleanIdLibReferencesFor(customBehaviour.id.objBt);
        }

        internal void LogIdLookup()
        {
           // LogLookup(_idLookup,"Idlookup");
        }

        internal void LogLookup()
        {
           // LogLookup(_behaviourLookup,"Behv Lookup");
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
                    if (ReferenceEquals(behaviours[i], null))
                    {
                        behaviours.RemoveAt(i);
                        continue;
                    }
                    
                    i++;
                }

            }
        }

        private void AppendBehavioursListInternal<T>(List<T> customBehaviours, Type t)
        {
            List<CustomBehaviour> oldComponents = _behaviourLookup[t];
            oldComponents.Capacity += customBehaviours.Count;

            for (int j = 0; j < customBehaviours.Count; j++)
            {
                CustomBehaviour newBehaviour = (CustomBehaviour)(object)customBehaviours[j];
                CheckAndAddToIdLookup(newBehaviour);
                oldComponents.Add(newBehaviour);
            }
        }

        private void AppendBehavioursArrayInternal<T>(T[] customBehaviours, Type t)
        {
            List<CustomBehaviour> oldComponents = _behaviourLookup[t];
            oldComponents.Capacity += customBehaviours.Length;

            for (int j = 0; j < customBehaviours.Length; j++)
            {
                CustomBehaviour behaviour = (CustomBehaviour)(object)customBehaviours[j];
                oldComponents.Add(behaviour);
                CheckAndAddToIdLookup(behaviour);
            }
        }

        private void AddListInternal<T>(List<T> customBehaviour, Type t)
        {
           List<CustomBehaviour> components = new List<CustomBehaviour>(customBehaviour.Count);
            
            for (int i = 0; i < customBehaviour.Count; i++)
            {
                CustomBehaviour behaviour = (CustomBehaviour)(object)customBehaviour[i];
                components[i] = behaviour;
                CheckAndAddToIdLookup(behaviour);
            }

            _behaviourLookup.Add(t, components);
        }
        
        private void AddArrayInternal<T>(T[] customBehaviour, Type t)
        {
            List<CustomBehaviour> components = new List<CustomBehaviour>(customBehaviour.Length);

            for (int i = 0; i < customBehaviour.Length; i++)
            {
                CustomBehaviour behaviour = (CustomBehaviour)(object)customBehaviour[i];
                components[i] = behaviour;
                CheckAndAddToIdLookup(behaviour);
            }

            _behaviourLookup.Add(t, components);
        }



        #endregion
       
    }
}
