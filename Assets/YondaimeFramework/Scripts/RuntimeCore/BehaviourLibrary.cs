

using System;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    public class BehaviourLibrary 
    {

        #region LOOKUPS
        private Dictionary<Type,CustomBehaviour[]> _behaviourLookup = new Dictionary<Type, CustomBehaviour[]>();
        private Dictionary<int,CustomBehaviour[]> _idLookup = new Dictionary<int, CustomBehaviour[]>();
        #endregion


        #region INITIALIZERS
        public void GenerateLibrary(CustomBehaviour[] behaviours)
        {
            _behaviourLookup.Clear();
            _idLookup.Clear();

            Dictionary<Type, List<CustomBehaviour>> tempLookup = new Dictionary<Type, List<CustomBehaviour>>();
            CheckForEmptyBehaviours();
            GenerateTempLookUps();
            FillInterfaceLookup();
            FillIdLookup();
            MoveTempToFinalLookup();

            void CheckForEmptyBehaviours()
            {
                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (behaviours[i] == null)
                        throw new Exception("Null object detected,Make sure to Scan library after making all scene edits");
                }
            }
            void GenerateTempLookUps()
            {
                for (int i = 0; i < behaviours.Length; i++)
                {
                    CustomBehaviour currentBehaviour = behaviours[i];
                    Type currentBehaviourType = currentBehaviour.GetType();
                    currentBehaviour.RefreshIds();

                    if (!tempLookup.ContainsKey(currentBehaviourType))
                    {
                        tempLookup.Add(currentBehaviourType, new List<CustomBehaviour>());
                    }
                    tempLookup[currentBehaviourType].Add(behaviours[i]);
                }
            }
            void FillInterfaceLookup()
            {

                Dictionary<Type, List<CustomBehaviour>> temp = new Dictionary<Type, List<CustomBehaviour>>();

                foreach (KeyValuePair<Type, List<CustomBehaviour>> item in tempLookup)
                {
                    Type[] interfaces = item.Key.GetInterfaces();
                    for (int i = 0; i < interfaces.Length; i++)
                    {
                        if (!temp.ContainsKey(interfaces[i]))
                        {
                            temp.Add(interfaces[i], item.Value);
                        }
                        else
                        {
                            temp[interfaces[i]].AddRange(item.Value);
                        }
                    }
                }

                foreach (KeyValuePair<Type, List<CustomBehaviour>> item in temp)
                {
                    if (!tempLookup.ContainsKey(item.Key))
                        tempLookup.Add(item.Key, new List<CustomBehaviour>());

                    tempLookup[item.Key].AddRange(item.Value);

                }
            }
            void FillIdLookup()
            {
                Dictionary<int, List<CustomBehaviour>> temp = new Dictionary<int, List<CustomBehaviour>>();

                foreach (KeyValuePair<Type, List<CustomBehaviour>> item in tempLookup)
                {
                    List<CustomBehaviour> items = item.Value;
                    int totalItems = item.Value.Count;


                    for (int i = 0; i < totalItems; i++)
                    {
                        int id = items[i].id.objBt;

                        if (id != ComponentId.None)
                        {
                            if (!temp.ContainsKey(id))
                                temp.Add(id, new List<CustomBehaviour>());

                            temp[id].Add(items[i]);
                        }
                    }
                }

                foreach (KeyValuePair<int, List<CustomBehaviour>> item in temp)
                {
                    _idLookup.Add(item.Key, item.Value.ToArray());
                }

            }
            void MoveTempToFinalLookup()
            {

                foreach (KeyValuePair<Type, List<CustomBehaviour>> item in tempLookup)
                {
                    _behaviourLookup.Add(item.Key, item.Value.ToArray());
                }

            }
        }

        internal void LogLookup()
        {
            string val = string.Empty;
            foreach (KeyValuePair<Type, CustomBehaviour[]> item in _behaviourLookup)
            {
                string v = $"Type {item.Key}, TotalInstances {item.Value.Length}  GOS=>[";
                for (int i = 0; i < item.Value.Length; i++)
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


        #region HANDLES

        public void AddBehaviour<T>(T newBehaviour)
        {
            Type t = typeof(T);
            if (_behaviourLookup.ContainsKey(t))
            {
                AppendBehaviour(newBehaviour, t);

            }
            else
            {
                _behaviourLookup.Add(t, new CustomBehaviour[] { (CustomBehaviour)(object)newBehaviour });
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
                    _behaviourLookup.Add(t, new CustomBehaviour[] { (CustomBehaviour)(object)newBehaviour });
                }


            }
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

        public void CleanReferencesFor<T>() 
        {
            Type t = typeof(T);

             CleanReferencesFor(t);

            Type[] itypes = t.GetInterfaces();
            for (int i = 0; i < itypes.Length; i++) 
            { 
                CleanReferencesFor(itypes[i]);
            }

        }

        #endregion



        #region INTERNAL_WORKERS

        private void AppendBehaviour<T>(T newBehaviour, Type t)
        {
            CustomBehaviour[] oldComponents = _behaviourLookup[t];
            CustomBehaviour[] components = new CustomBehaviour[oldComponents.Length + 1];

            int oldCount = oldComponents.Length;

            for (int i = 0; i < oldCount; i++)
            {
                components[i] = oldComponents[i];
            }

            components[oldCount] = (CustomBehaviour)(object)newBehaviour;
            _behaviourLookup[t] = components;
        }

        private void CleanReferencesFor(Type t)
        {

            if (_behaviourLookup.ContainsKey(t))
            {
                CustomBehaviour[] behaviours = _behaviourLookup[t];
                int reducedCount = behaviours.Length;

                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (behaviours[i] == null)
                        reducedCount--;
                }

                CustomBehaviour[] newBehaviours = new CustomBehaviour[reducedCount];

                int c = 0;
                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (behaviours[i] != null)
                    {
                        newBehaviours[c] = behaviours[i];
                        c++;
                    }
                }

                _behaviourLookup[t] = newBehaviours;
            }
        }

        private void AppendBehavioursListInternal<T>(List<T> customBehaviour, Type t)
        {
            CustomBehaviour[] oldComponents = _behaviourLookup[t];
            CustomBehaviour[] components = new CustomBehaviour[oldComponents.Length + customBehaviour.Count];

            int oldCount = oldComponents.Length;
            int i;
            for (i = 0; i < oldCount; i++)
            {
                components[i] = oldComponents[i];
            }

            int k = 0;
            for (int j = i; j < components.Length; j++)
            {
                components[j] = (CustomBehaviour)(object)customBehaviour[k];
                k++;
            }

            // Debug.Log($"{oldComponents.Length} {components.Length}  {oldCount}");

            _behaviourLookup[t] = components;
        }

        private void AppendBehavioursArrayInternal<T>(T[] customBehaviour, Type t)
        {
            CustomBehaviour[] oldComponents = _behaviourLookup[t];
            CustomBehaviour[] components = new CustomBehaviour[oldComponents.Length + customBehaviour.Length];

            int oldCount = oldComponents.Length;
            int i;
            for (i = 0; i < oldCount; i++)
            {
                components[i] = oldComponents[i];
            }

            int k = 0;
            for (int j = i; j < components.Length; j++)
            {
                components[j] = (CustomBehaviour)(object)customBehaviour[k];
                k++;
            }

            // Debug.Log($"{oldComponents.Length} {components.Length}  {oldCount}");

            _behaviourLookup[t] = components;
        }

        private void AddListInternal<T>(List<T> customBehaviour, Type t)
        {
            CustomBehaviour[] components = new CustomBehaviour[customBehaviour.Count];
            for (int i = 0; i < customBehaviour.Count; i++)
            {
                components[i] = (CustomBehaviour)(object)customBehaviour[i];
            }
            _behaviourLookup.Add(t, components);
        }
        
        private void AddArrayInternal<T>(T[] customBehaviour, Type t)
        {
            CustomBehaviour[] components = new CustomBehaviour[customBehaviour.Length];
            for (int i = 0; i < customBehaviour.Length; i++)
            {
                components[i] = (CustomBehaviour)(object)customBehaviour[i];
            }

            _behaviourLookup.Add(t, components);
        }
        


        #endregion

    }
}
