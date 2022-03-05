using YondaimeFramework;
using System.Collections.Generic;
using System;

namespace YondaimeFramework
{
    public class StandardBehaviourLibrary : BehaviourLibraryOld
    {
        //NonSerialized
        private Dictionary<Type, CustomBehaviour[]> _behaviourLookup = new Dictionary<Type, CustomBehaviour[]>();
        private Dictionary<int, CustomBehaviour[]> _idLookup = new Dictionary<int, CustomBehaviour[]>();



        #region PUBLIC_METHODS
        public override void InitializeLibrary()
        {
            _behaviourLookup.Clear();
            _idLookup.Clear();

            Dictionary<Type, List<CustomBehaviour>> tempLookup = new Dictionary<Type, List<CustomBehaviour>>();
            CheckForEmptyBehaviours();
            GenerateTempLookUps();
            FillInterfaceLookup();
            FillIdLookup();
            MoveTempToFinalLookup();
            InitChildLibraries();

            void CheckForEmptyBehaviours()
            {
                for (int i = 0; i < _behaviours.Count; i++)
                {
                    if (_behaviours[i] == null)
                        throw new Exception("Null object detected,Make sure to Scan library after making all scene edits");
                }
            }

            void GenerateTempLookUps()
            {
                for (int i = 0; i < _behaviours.Count; i++)
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

            void InitChildLibraries()
            {
                for (int i = 0; i < _childLibs.Length; i++)
                {
                    _childLibs[i].InitializeLibrary();
                }
            }

        }

        public override T GetBehaviourFromLibrary<T>()
        {
            Type reqeuestedType = typeof(T);

            if (_behaviourLookup.ContainsKey(reqeuestedType))
                return (T)(object)_behaviourLookup[reqeuestedType][0];

            for (int i = 0; i < _childLibs.Length; i++)
            {
                T behaviour = _childLibs[i].GetBehaviourFromLibrary<T>();
                if (behaviour != null)
                    return behaviour;
            }

            return default;
        }

        public override T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
            Type reqeuestedType = typeof(T);

            if (_behaviourLookup.ContainsKey(reqeuestedType))
            {
                CustomBehaviour[] behaviours = _behaviourLookup[reqeuestedType];

                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (behaviours[i].GOInstanceId == requesteeGameObjectInstanceId)
                    {
                        return (T)(object)behaviours[i];
                    }
                }
            }


            for (int i = 0; i < _childLibs.Length; i++)
            {
                T behaviour = _childLibs[i].GetBehaviourOfGameObject<T>(requesteeGameObjectInstanceId);
                if (behaviour != null)
                    return behaviour;
            }

            return default;




        }

        public override T GetBehaviourFromLibraryById<T>(int behaviourId)
        {

            if (_idLookup.ContainsKey(behaviourId))
            {
                CustomBehaviour[] behv = _idLookup[behaviourId];

                for (int i = 0; i < behv.Length; i++)
                {
                    if (behv[i] is T)
                        return (T)(object)behv[i];
                }

            }

            for (int i = 0; i < _childLibs.Length; i++)
            {
                T behaviour = _childLibs[i].GetBehaviourFromLibraryById<T>(behaviourId);
                if (behaviour != null)
                    return behaviour;
            }


            return default;
        }

        public override void GetBehavioursFromLibrary<T>(List<T> behaviourListToBeFilled)
        {
            Type reqeuestedType = typeof(T);

            if (_behaviourLookup.ContainsKey(reqeuestedType))
            {
                CustomBehaviour[] behavioursInLookUp = _behaviourLookup[reqeuestedType];
                behaviourListToBeFilled.Capacity += behavioursInLookUp.Length;

                for (int i = 0; i < behavioursInLookUp.Length; i++)
                {
                    behaviourListToBeFilled.Add((T)(object)behavioursInLookUp[i]);
                }
            }

            for (int i = 0; i < _childLibs.Length; i++)
            {
                _childLibs[i].GetBehavioursFromLibrary(behaviourListToBeFilled);
            }
        }

        public override void GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId, List<T> behaviourListToBeFilled)
        {
            Type reqeuestedType = typeof(T);

            if (_behaviourLookup.ContainsKey(reqeuestedType))
            {
                CustomBehaviour[] behavioursInLookUp = _behaviourLookup[reqeuestedType];
                behaviourListToBeFilled.Capacity += behavioursInLookUp.Length;

                for (int i = 0; i < behavioursInLookUp.Length && behavioursInLookUp[i].GOInstanceId == requesteeGameObjectInstanceId; i++)
                {
                    behaviourListToBeFilled.Add((T)(object)behavioursInLookUp[i]);
                }

            }


            for (int i = 0; i < _childLibs.Length; i++)
            {
                _childLibs[i].GetBehavioursOfGameObject<T>(requesteeGameObjectInstanceId, behaviourListToBeFilled);
            }

        }
        #endregion
    }
}
