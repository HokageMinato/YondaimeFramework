using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;
using System.Diagnostics;


    public class GetCompByIdvsFindByTag : CustomBehaviour,IProfileComponent
    {
        private string classResult;
        private string intefaceResult;
        private int iterationCount;
        public CustomBehaviour targetObject;
        public ComponentId idToSearch;
        public const string TagToSearch = "Finish";

        public string GetClassResult()
        {
            return classResult;
        }

        public string GetInterfaceResult()
        {
            return intefaceResult;
        }

        public void SetIterations(int interations)
        {
            iterationCount = interations;
        }

        public void TestClass()
        {

            string res;

            Stopwatch st = new Stopwatch();
            st.Start();
            for (double i = 0; i < iterationCount; i++)
            {
                GetComponentFromLibraryById<SportsCar>(idToSearch);
            }
            st.Stop();
            res = st.ElapsedMilliseconds.ToString();


            st.Reset();
            st.Start();
            for (double i = 0; i < iterationCount; i++)
            {
                GameObject.FindGameObjectWithTag(TagToSearch);
            }
            st.Stop();

            classResult = $"{res} <C U> {st.ElapsedMilliseconds}";

        }

        public void TestInterface()
        {
            intefaceResult = ($"NA <C U> N/A");
        }
    }
