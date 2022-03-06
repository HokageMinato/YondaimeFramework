using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YondaimeFramework;
using MessagePack;

namespace YondaimeFramework
{
    public class SceneLibrary : MonoBehaviour
    {

        public List<BehaviourLibrary> _libraries;
        public List<CustomBehaviour> _behaviours;
        public byte[] componentAllocationMap;

        private Dictionary<Type, int> behaviourAllocationCountsByType;


        #region RUNTIME_LOOKUP_GENERATION
        #endregion

        #region BEHAVIOUR_LIBRARY_GENERATION
        public void GenerateBehaviourLibrary()
        {
            _libraries.Clear();
            _behaviours.Clear();

            List<BehaviourLibrary> libraries = new List<BehaviourLibrary>(GetComponentsInChildren<BehaviourLibrary>());
            List<CustomBehaviour> behaviours = new List<CustomBehaviour>(GetComponentsInChildren<CustomBehaviour>());
            List<PooledBehaviourLibrary> poolLibraries = new List<PooledBehaviourLibrary>(GetComponentsInChildren<PooledBehaviourLibrary>());

            InitLibraries(libraries);
            TakeBehaviourTypeObjectCounts(behaviours);
            SetSceneLibraryInBehaviours(behaviours);
            //SeperateBehaviours(libraries,behaviours);
            //SeperateLibraries(libraries);
            
        }

        void TakeBehaviourTypeObjectCounts(List<CustomBehaviour> behaviours)
        {
            behaviourAllocationCountsByType = new Dictionary<Type, int>();

            for (int i = 0; i < behaviours.Count; i++)
            {
                CustomBehaviour b = behaviours[i];
                Type typeName = b.GetType();
                Type[] interfaces = typeName.GetInterfaces();

                AddtoAllocationCount(typeName);

                for (int j = 0; j < interfaces.Length; j++)
                {
                    Type iTypeName = interfaces[j];
                    AddtoAllocationCount(iTypeName);
                }
            }

           if(!Application.isPlaying)
            componentAllocationMap =  MessagePackSerializer.Serialize(behaviourAllocationCountsByType);
        }

        void AddtoAllocationCount(Type type) 
        {
            if (!behaviourAllocationCountsByType.ContainsKey(type))
                behaviourAllocationCountsByType.Add(type, 0);

            behaviourAllocationCountsByType[type]++;
        }

        void SetSceneLibraryInBehaviours(List<CustomBehaviour> behaviours) 
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                behaviours[i].SetLibrary(this);
            }
        }

        void InitLibraries(List<BehaviourLibrary> libraries)
        {
            for (int i = 0; i < libraries.Count; i++)
                libraries[i].InitLibrary();
        }

        void SeperateBehaviours(List<BehaviourLibrary> libraries, List<CustomBehaviour> behaviours)
        {

            for (int i = 0; i < behaviours.Count; i++)
            {
                CustomBehaviour customBehaviour = behaviours[i];
                BehaviourLibrary parentLibrary = FindLibByTransform(customBehaviour.FindLibParentTransform(), libraries);

                if (parentLibrary != null)
                {
                    customBehaviour.SetLibrary(parentLibrary);
                    parentLibrary.AddBehaviour(customBehaviour);
                    continue;
                }
                
                _behaviours.Add(customBehaviour);
            }

        }

        void SeperateLibraries(List<BehaviourLibrary> libraries)
        {
            for (int i = 0; i < libraries.Count; i++)
            {
                BehaviourLibrary behaviourLibrary = libraries[i];
                BehaviourLibrary parentLibrary = FindLibByTransform(behaviourLibrary.FindLibParentTransform(), libraries); 

                if (parentLibrary)
                {
                    parentLibrary.AddLibrary(behaviourLibrary);
                    continue;
                }

                _libraries.Add(behaviourLibrary);
            }

        }

        BehaviourLibrary FindLibByTransform(Transform t, List<BehaviourLibrary> libraries) 
        {
            for (int i = 0; i < libraries.Count; i++)
            {
                BehaviourLibrary lib = libraries[i];
                if (lib.transform == t)
                    return lib;
            }

            return null;
        }
        #endregion

    }

    //[System.Serializable]
    //public class SLWDictionary<K, V>
    //{
    //    public K[] _Keys;
    //    public V[] _Values;

    //    public SLWDictionary(Dictionary<K, V> runtimeDictionary)
    //    {
    //        int valueCount = runtimeDictionary.Count;
    //        _Keys = new K[valueCount];
    //        _Values = new V[valueCount];

    //        int c = 0;
    //        foreach (KeyValuePair<K, V> item in runtimeDictionary)
    //        {
    //            _Keys[c] = item.Key;
    //            _Values[c] = item.Value;
    //            c++;
    //        }

    //    }
        
    //    public void Add (Dictionary<K, V> runtimeDictionary)
    //    {
    //        int valueCount = runtimeDictionary.Count-1;

    //        K[] keys = new K[_Keys.Length + valueCount];
    //        V[] values = new V[_Keys.Length + valueCount];

    //        for (int i = 0; i < _Keys.Length; i++)
    //        {
    //            keys[i] = _Keys[i];
    //        }
    //        for (int i = 0; i < _Keys.Length; i++)
    //        {
    //            values[i] = _Values[i];
    //        }

    //        int c = _Keys.Length - 1;
    //        foreach (KeyValuePair<K, V> item in runtimeDictionary)
    //        {
    //            keys[c] = item.Key;
    //            values[c] = item.Value;
    //            c++;
    //        }


    //        _Keys = keys;
    //        _Values = values;
    //    }

        

    //    public Dictionary<K,V> GetRuntimeVariant() 
    //    {
    //        Dictionary<K, V> dict = new Dictionary<K, V>();
    //        int count = _Keys.Length;

    //        for (int i = 0; i < count; i++)
    //        {
    //            dict.Add(_Keys[i], _Values[i]);
    //        }
            
    //        return dict;
    //    }


        


    //    public void Clear() 
    //    {
    //        _Keys = null;
    //        _Values = null;
    //    }
    //}
    
}