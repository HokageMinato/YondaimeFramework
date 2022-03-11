using YondaimeFramework;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace YondaimeFramework
{
    public class StandardBehaviourLibraryOld<T> where T : class
    {
        //NonSerialized
        
        private Dictionary<Type, T[]> _behaviourLookup = new Dictionary<Type, T[]>();
        private Dictionary<int, T[]> _idLookup = new Dictionary<int, T[]>();



        #region PUBLIC_METHODS
        public void InitializeLibrary(CustomBehaviour[] _behaviours)
        {
            _behaviourLookup.Clear();
            _idLookup.Clear();

            Dictionary<Type, List<CustomBehaviour>> tempLookup = new Dictionary<Type, List<CustomBehaviour>>();
            CheckForEmptyBehaviours();
            GenerateTempLookUps();
            FillInterfaceLookup();
            MoveTempToFinalLookup();

            void CheckForEmptyBehaviours()
            {
                for (int i = 0; i < _behaviours.Length; i++)
                {
                    if (_behaviours[i] == null)
                        throw new Exception("Null object detected,Make sure to Scan library after making all scene edits");
                }
            }
            void GenerateTempLookUps()
            {
                for (int i = 0; i < _behaviours.Length; i++)
                {
                    CustomBehaviour currentBehaviour = _behaviours[i];
                    Type currentBehaviourType = currentBehaviour.GetType();
                    currentBehaviour.RefreshIds();

                    if (!tempLookup.ContainsKey(currentBehaviourType))
                    {
                        tempLookup.Add(currentBehaviourType, new List<CustomBehaviour>());
                    }
                    tempLookup[currentBehaviourType].Add(_behaviours[i]);
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
            void MoveTempToFinalLookup()
            {

                foreach (KeyValuePair<Type, List<CustomBehaviour>> item in tempLookup)
                {
                    List<T> items = new List<T>(item.Value.Count);
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        items.Add((T)(object)item.Value[i]);
                    }

                    _behaviourLookup.Add(item.Key, items.ToArray());

                }

            }
        }

        public T GetBehaviourFromLibrary<K>()
        {
            Type reqeuestedType = typeof(K);

            if (_behaviourLookup.ContainsKey(reqeuestedType))
                return _behaviourLookup[reqeuestedType][0];

            return default;
        }

        public K GetBehaviourOfGameObject<K>(int requesteeGameObjectInstanceId)
        {
            Type reqeuestedType = typeof(K);

            if (_behaviourLookup.ContainsKey(reqeuestedType))
            {

                
            }
            return default;

        }

        public T GetBehaviourFromLibraryById<K>(int behaviourId)
        {
            if (_idLookup.ContainsKey(behaviourId))
            {
                T[] behv = _idLookup[behaviourId];

                for (int i = 0; i < behv.Length; i++)
                {
                    if (behv[i] is K)
                        return behv[i];
                }

            }


            return default;
        }

        public K[] GetBehavioursFromLibrary<K>()
        {
            Type reqeuestedType = typeof(K);
            Type reqeuestedTyper = typeof(K[]);

            //if (_behaviourLookup.ContainsKey(reqeuestedType))
            //{
            //    return Convert.ChangeType(_behaviourLookup[reqeuestedType],reqeuestedTyper);
            //}
            return null;
        }

        
        #endregion
    }
}
