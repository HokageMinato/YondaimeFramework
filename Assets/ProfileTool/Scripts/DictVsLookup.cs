using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;
using System;
using System.Linq;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
using System.Diagnostics;

    public class DictVsLookup : CustomBehaviour,IProfileComponent
    {
         Dictionary<Type, CustomBehaviour[]> ttempLookup = new Dictionary<Type, CustomBehaviour[]>();
         ILookup<Type, CustomBehaviour> lookup;
         List<Type> testQ = new List<Type>();
            public int itr;

        Type[] acc;


    void Start() 
    {
        GenerateTempLookUps();
    }

            void GenerateTempLookUps()
            {
                Dictionary<Type, List<CustomBehaviour>> tempLookup = new Dictionary<Type, List<CustomBehaviour>>();
                CustomBehaviour[] behaviours = FindObjectsOfType<CustomBehaviour>();

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

                foreach (var item in tempLookup)
                {
                    testQ.Add(item.Key);
                    ttempLookup.Add(item.Key, item.Value.ToArray());
                }

                lookup = tempLookup
                 .SelectMany(p => p.Value, Tuple.Create)
                 .ToLookup(p => p.Item1.Key, p => p.Item2);
                Type reqTyp = GetType();


                

            }

           

    public void SetIterations(int interations)
    {
        itr = interations;

        acc = new Type[itr];

        List<Type> ttemp = new List<Type>();
        for (int i = 0; i < itr; i++)
        {
            ttemp.Add(testQ[Random.Range(0, testQ.Count)]);
        }
        acc = ttemp.ToArray();
    }

    public string GetClassResult()
    {
        string res;
        

        Stopwatch st = new Stopwatch();
        st.Start();
        for (int i = 0; i < acc.Length; i++)
        {
            t.AddRange(lookup[acc[i]]);
        }
        st.Stop();
        res = st.ElapsedMilliseconds.ToString();
        t.Clear();
        return ($"Lookp access {res}");

    }

    List<CustomBehaviour> t = new List<CustomBehaviour>();

    public string GetInterfaceResult()
    {
        string res;
        Stopwatch st = new Stopwatch();
        st.Start();
        for (int i = 0; i < acc.Length; i++)
        {
            t.AddRange(ttempLookup[acc[i]]);
        }
        st.Stop();
        res = st.ElapsedMilliseconds.ToString();
        t.Clear();
       return ($"Dict access {res}");
    }

    public void TestClass()
    {
        
    }

    public void TestInterface()
    {
        
    }
}
