using System.Collections.Generic;
using System;


namespace YondaimeFramework
{

    

    public class PooledBehaviourLibrary : BehaviourLibrary
    {
        

        private Dictionary<Type, PerformancePool<CustomBehaviour>> _behaviourLookup = new Dictionary<Type, PerformancePool<CustomBehaviour>>();
        private Dictionary<int, CustomBehaviour[]> _idLookup = new Dictionary<int, CustomBehaviour[]>();

        public override void InitializeLibrary()
        {
            _behaviourLookup.Clear();
            _idLookup.Clear();

            Dictionary<Type, PerformancePool<CustomBehaviour>> tempLookup = new Dictionary<Type, PerformancePool<CustomBehaviour>>();
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
                        tempLookup.Add(currentBehaviourType, new PerformancePool<CustomBehaviour>());
                    }

                    if (_behaviours[i].PoolState == active)
                        tempLookup[currentBehaviourType].AddAsDePooledState(_behaviours[i]);
                    else
                        tempLookup[currentBehaviourType].AddAsPooledState(_behaviours[i]);
                }
            }
            void FillInterfaceLookup()
            {

                Dictionary<Type, PerformancePool<CustomBehaviour>> temp = new Dictionary<Type,PerformancePool<CustomBehaviour>>();

                foreach (KeyValuePair<Type, PerformancePool<CustomBehaviour>> item in tempLookup)
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

                foreach (KeyValuePair<Type, PerformancePool<CustomBehaviour>> item in temp)
                {
                    if (!tempLookup.ContainsKey(item.Key))
                        tempLookup.Add(item.Key, new PerformancePool<CustomBehaviour>());

                    tempLookup[item.Key].AddRange(item.Value);

                }
            }
            void FillIdLookup()
            {
                Dictionary<int, List<CustomBehaviour>> temp = new Dictionary<int, List<CustomBehaviour>>();

                foreach (KeyValuePair<Type, PerformancePool<CustomBehaviour>> item in tempLookup)
                {
                    List<CustomBehaviour> items = item.Value.ToList();
                    int totalItems = items.Count;


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

                foreach (KeyValuePair<Type, PerformancePool<CustomBehaviour>> item in tempLookup)
                {
                    _behaviourLookup.Add(item.Key, item.Value);
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
                return (T)(object)_behaviourLookup[reqeuestedType][0][0];

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
                CustomBehaviour[] behaviours = _behaviourLookup[reqeuestedType][0];

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
            throw new System.NotImplementedException();
        }

       

        public override void GetBehavioursFromLibrary<T>(List<T> behaviourListToBeFilled)
        {
            throw new System.NotImplementedException();
        }

        public override void GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId, List<T> behaviourListToBeFilled)
        {
            throw new System.NotImplementedException();
        }

        
    }


    public class PerformancePool<T>
    {
        //public List<T> inActiveObjects = new List<T>();
        //public List<T> activeObjects = new List<T>();
        public const int Active = 0;
        public const int Pooled = 1;

        public List<List<T>> objects = new List<List<T>>(); 

        public List<T> this[int idx]
        {
            get
            {
                return objects[idx];
            }
        }

        public void AddAsDePooledState(T behaviour) 
        { 
            objects[Active].Add(behaviour);
        }
        
        public void AddAsPooledState(T behaviour) 
        { 
            objects[Pooled].Add(behaviour);
        }

        public void AddRange(PerformancePool<T> otherPool) 
        { 
            objects[Active].AddRange(otherPool.objects[Active]);
            objects[Pooled].AddRange(otherPool.objects[Pooled]);
        }

        public void Pool(T behaviour)
        {
            objects[Pooled].Add(behaviour);
        }

        public T GetPooled()
        {
            int objectCount = objects[Pooled].Count;

            if (objectCount <= 0)
                return default;

            T ob = objects[Pooled][objectCount - 1];
            objects[Pooled].RemoveAt(objectCount - 1);
            return ob;
        }

        public List<T> ToList() 
        { 
            List<T> list = new List<T>(objects[Pooled].Count);
            list.Capacity += objects[Active].Count;
            list.AddRange(objects[Active]);
            list.AddRange(objects[Pooled]);
            return list;    
        }
    
    }

}
