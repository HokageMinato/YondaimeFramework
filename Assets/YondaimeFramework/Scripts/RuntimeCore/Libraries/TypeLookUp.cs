using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YondaimeFramework
{
    public class TypeLookUp 
    {
        //SubclassMap Strategy
        #region LOOKUP
        private Dictionary<Type, IList> _behaviourLookup = new Dictionary<Type, IList>();
        #endregion


        #region INITIALIZER
        public void GenerateLookUp(CustomBehaviour[] behaviours) 
        {
            CheckForEmptyBehaviours();
            FillLookUp();

            void CheckForEmptyBehaviours()
            {
                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (behaviours[i] == null)
                        throw new Exception("Null object detected,Make sure to Scan library after making all scene edits");

                    behaviours[i].RefreshIds();
                }
            }
            void FillLookUp()
            {
                for (int i = 0; i < behaviours.Length; i++)
                {
                    CustomBehaviour currentBehaviour = behaviours[i];
                    AddBehaviour(currentBehaviour);
                }
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
            AddToLookup(newBehaviour, t);

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++)
            {
                AddToLookup(newBehaviour, itypes[i]);
            }

            List<Type> baseTypes = GetBaseTypes(t);
            for (int i = 0; i < baseTypes.Count; i++)
            {
                AddToLookup(newBehaviour, baseTypes[i]);
            }
        }

        private void AddToLookup(CustomBehaviour newBehaviour, Type t)
        {
            if (!_behaviourLookup.ContainsKey(t))
            {
                _behaviourLookup.Add(t, GenerateListOfType(t));
            }

            if (!_behaviourLookup[t].Contains(newBehaviour))
                _behaviourLookup[t].Add(newBehaviour);
                
        }

        public void CleanAllNullReferencesOnSceneChange() 
        {
            foreach (KeyValuePair<Type, IList> item in _behaviourLookup)
            {
                CleanNullReferencesFor(item.Value);
            }
        }

        
        public void CleanNullReferencesFor(IList behaviours)
        {
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


        public void CleanReferencesExplicitlyOf(CustomBehaviour behaviour) 
        {
            Type t = behaviour.GetType();
            CleanReferenceExplicitlyOf(behaviour,t);

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++)
            {
                CleanReferenceExplicitlyOf(behaviour,itypes[i]);
            }

            List<Type> types = GetBaseTypes(t);
            for (int i = 0; i < types.Count; i++)
            {
                CleanReferenceExplicitlyOf(behaviour,types[i]);
            }

        }

        public void AppendLookUp(TypeLookUp otherLookup) 
        {
            Dictionary<Type, IList> newBehaviours = otherLookup._behaviourLookup;

            foreach (var item in newBehaviours)
            {
                if (!_behaviourLookup.ContainsKey(item.Key))
                {
                    _behaviourLookup.Add(item.Key, item.Value);
                }
                else 
                { 
                    _behaviourLookup[item.Key].Add(item.Value);
                }
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

        
        List<Type> GetBaseTypes(Type type)
        {
            List<Type> baseTypes = new List<Type>();
            
            while (type.BaseType != typeof(CustomBehaviour))
            {
                baseTypes.Add(type.BaseType);
                type = type.BaseType;
            }

            return baseTypes;
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
