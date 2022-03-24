using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

    public class Truck : MonoBehaviour
    {
        public int itr;
        private Dictionary<int, List<CustomBehaviour>> _idLookup= new Dictionary<int, List<CustomBehaviour>>();
        private Dictionary<int,Dictionary<Type,CustomBehaviour>> _idLookupSquared= new Dictionary<int, Dictionary<Type, CustomBehaviour>>();
        public ComponentId idToGet;
        public CustomBehaviour[] cars;

        

    private void Start()
    {
        
    }


    [ContextMenu("Set")]
    void Set() 
    {

        AddToIdLookup(cars[0]);
        AddToIdLookup(cars[1]);
        AddBehaviourInterfacesInLookup(cars[0]);
        AddBehaviourInterfacesInLookup(cars[1]);

        Debug.Log(GetBehaviourFromLibraryById<ICar>(idToGet.objBt) == null);
        Debug.Log(GetBehaviourFromLibraryById<ICar>(idToGet.objBt) == null);

        Debug.Log(_idLookup[idToGet.objBt].Count);
        Debug.Log(_idLookupSquared[idToGet.objBt].Count);

    }

     void AddBehaviourInterfacesInLookup(CustomBehaviour behaviour)
    {
                Type t = behaviour.GetType();
                int id = behaviour.id.objBt;
                Type[] itypes = t.GetInterfaces();
                for (int i = 0; i < itypes.Length; i++)
                {
                    if (!_idLookupSquared.ContainsKey(id))
                    {
                        _idLookupSquared.Add(id, new Dictionary<Type, CustomBehaviour> { { itypes[i], behaviour } });
                        continue;
                    }

                    if(!_idLookupSquared[id].ContainsKey(itypes[i]))
                       _idLookupSquared[id].Add(itypes[i], behaviour);

                }
    }

    void AddToIdLookup(CustomBehaviour behaviour)
    {
        int id = behaviour.id.objBt;

        if (!_idLookup.ContainsKey(id))
        {
            _idLookup.Add(id, new List<CustomBehaviour>());
        }
        _idLookup[id].Add(behaviour);


        if (!_idLookupSquared.ContainsKey(id))
        {
            _idLookupSquared.Add(id, new Dictionary<Type, CustomBehaviour> { { behaviour.GetType(), behaviour } });
        }


    }


   


    [ContextMenu("Test Instantiate Destory")]
    public void InvocationTest()
    {
        string data = string.Empty;

        Stopwatch st = new Stopwatch();
        st.Start();
        for (int i = 0; i < itr; i++)
        {
            GetBehaviourFromLibraryByIdAF<ICar>(idToGet.objBt);
        }

        st.Stop();
        data += $"LKPLKP {st.ElapsedMilliseconds} - ";

        //   ml.LogLookup();
        //  ml.LogIdLookup();


        st.Reset();
        st.Start();


        for (int i = 0; i < itr; i++)
        {
            GetBehaviourFromLibraryById<ICar>(idToGet.objBt);
        }

        st.Stop();

        // ml.LogLookup();
        // ml.LogIdLookup();


        data += $" LKPLST {st.ElapsedMilliseconds}";
        Debug.Log(data);

    }




    public T GetBehaviourFromLibraryById<T>(int behaviourId)
    {

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
    
    public T GetBehaviourFromLibraryByIdAF<T>(int behaviourId)
    {
        return (T)(object) _idLookupSquared[behaviourId][typeof(T)];
    }


}
