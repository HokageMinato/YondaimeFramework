using System;
using System.Collections.Generic;
using UnityEngine;


 namespace YondaimeFramework
{
    public class BehaviourLibrary : CustomBehaviour
    {

        #region PRIVATE_VARIABLES
        //Serialized
        [SerializeField] protected List<CustomBehaviour> _behaviours;
        [SerializeField] protected BehaviourLibrary[] _childLibs;

        //NonSerialized
        private Dictionary<Type, CustomBehaviour[]> _behaviourLookup = new Dictionary<Type, CustomBehaviour[]>();
        private Dictionary<int, CustomBehaviour[]> _idLookup = new Dictionary<int, CustomBehaviour[]>();
        #endregion



        #region PUBLIC_METHODS
        public void InitializeLibrary()
        {
            _behaviourLookup.Clear();
            _idLookup.Clear();

            Dictionary<Type, List<CustomBehaviour>> tempLookup = new Dictionary<Type, List<CustomBehaviour>>();
            GenerateTempLookUps();
            FillInterfaceLookup();
            FillIdLookup();
            MoveTempToFinalLookup();
            InitChildLibraries();


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
                            if(!temp.ContainsKey(id))
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



        public T GetBehaviourFromLibrary<T>()
        {
            Type reqeuestedType = typeof(T);

            if (_behaviourLookup.ContainsKey(reqeuestedType))
                return (T)(object)_behaviourLookup[reqeuestedType][0];

            for (int i = 0; i < _childLibs.Length; i++)
            {
                T behaviour =_childLibs[i].GetBehaviourFromLibrary<T>();
                if (behaviour != null)
                    return behaviour;
            }

            return default;
        }

        public T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId) 
        {
            Type reqeuestedType = typeof(T);

            if (_behaviourLookup.ContainsKey(reqeuestedType))
            {
                CustomBehaviour[] behaviours = _behaviourLookup[reqeuestedType];

                for (int i = 0; i < behaviours.Length; i++)
                {
                    if (behaviours[i].GOInstanceId == requesteeGameObjectInstanceId) {
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

        
        public T GetBehaviourFromLibraryById<T>(int behaviourId) 
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

        public void GetBehavioursFromLibrary<T>(List<T> behaviourListToBeFilled)
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

        public void GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId,List<T> behaviourListToBeFilled)
        {
            Type reqeuestedType = typeof(T);
          
            if (_behaviourLookup.ContainsKey(reqeuestedType))
            {
                CustomBehaviour[] behavioursInLookUp = _behaviourLookup[reqeuestedType];
                behaviourListToBeFilled.Capacity += behavioursInLookUp.Length;

                for (int i = 0; i < behavioursInLookUp.Length && behavioursInLookUp[i].GOInstanceId ==  requesteeGameObjectInstanceId; i++)
                {
                    behaviourListToBeFilled.Add((T)(object)behavioursInLookUp[i]);
                 }
                
             }


            for (int i = 0; i < _childLibs.Length; i++)
            {
                _childLibs[i].GetBehavioursOfGameObject<T>(requesteeGameObjectInstanceId,behaviourListToBeFilled);
            }
            
        }

       
        
        public void LogLibrary() {
            string items = "";
            foreach (var item in _behaviours)
            {
                items+=item.GetType()+"\n";

            }
            FrameworkLogger.Log(items);
        }

        public virtual void ScanBehaviours()
        {
            _behaviours = new List<CustomBehaviour>(GetComponentsInChildren<CustomBehaviour>(true));
            PreRedundantCheck();
            ScanChildLibs();
            RemoveRedundantBehavioursRecursive();
            RemoveRedundantChildLibRecursive();
            SetActiveLibraries();


            void ScanChildLibs()
            {

                List<BehaviourLibrary> tempLibs = new List<BehaviourLibrary>();
                for (int i = 0; i < _behaviours.Count; i++)
                {
                    if (_behaviours[i] is BehaviourLibrary && !_behaviours[i].Equals(this))
                    {
                        tempLibs.Add((BehaviourLibrary)_behaviours[i]);
                    }
                }
                _childLibs = tempLibs.ToArray();


                for (int i = 0; i < _childLibs.Length; i++)
                {
                    _childLibs[i].ScanBehaviours();
                }

            }
            void RemoveRedundantChildLibRecursive()
            {

                List<BehaviourLibrary> myLibs = new List<BehaviourLibrary>(_childLibs);

                for (int i = 0; i < _childLibs.Length; i++)
                {
                    for (int j = 0; j < _childLibs[i]._childLibs.Length; j++)
                    {
                        myLibs.Remove(_childLibs[i]._childLibs[j]);
                        _childLibs[i].ScanBehaviours();
                    }
                }

                _childLibs = myLibs.ToArray();
            }
            void RemoveRedundantBehavioursRecursive()
            {

                for (int i = 0; i < _childLibs.Length; i++)
                {
                    for (int j = 0; j < _childLibs[i]._behaviours.Count; j++)
                    {
                        _behaviours.Remove(_childLibs[i]._behaviours[j]);
                    }
                }


            }
            void SetActiveLibraries()
            {
                for (int i = 0; i < _behaviours.Count; i++)
                {
                    _behaviours[i].SetLibrary(this);
                    _behaviours[i].SetLibrary(MySceneLibrary);
                }
            }
        }

        public virtual void PreRedundantCheck() { }

        private bool IsCustomId(byte behaviourId) 
        {
            return behaviourId > 0;
            //return (!string.IsNullOrEmpty(behaviourId) &&
            //                !string.IsNullOrWhiteSpace(behaviourId)) ;
        }
        #endregion
    }
};