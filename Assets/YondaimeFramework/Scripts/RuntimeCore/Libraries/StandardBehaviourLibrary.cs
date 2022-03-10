using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    public class StandardBehaviourLibrary : ILibrary
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
                 CustomBehaviour behaviour = behaviours[i];
                 if (behaviour.id._goInsId == requesteeGameObjectInstanceId)
                     return (T)(object)behaviour;
                
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
                CustomBehaviour behaviour = behv[i];    
                    if (behaviour is T)
                        return (T)(object)behaviour;
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

            for (int i = 0; i < objectCount; i++)
            {
                CustomBehaviour behaviour = behavioursInLookUp[i];
                
                if(behaviour.id._goInsId == requesteeGameObjectInstanceId)
                    returnList.Add((T)(object)behaviour);
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

        public void LogIdLookup()
        {
           // LogLookup(_idLookup,"Idlookup");
        }

        public void LogLookup()
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

        public T GetPooled<T>()
        {
            throw new Exception("No components are pooled natively in Standard Library");
        }

        public void Pool<T>(T behaviour)
        {
            throw new Exception("Pooling components natively in Standard Library is Not Allowed");
        }

        #endregion

    }
}
