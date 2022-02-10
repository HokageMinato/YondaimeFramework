using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;
using System.Diagnostics;
  public class GetComponentsFromGOvsGetComponents : CustomBehaviour,IProfileComponent
    {

        private string classResult;
        private string intefaceResult;
        private int iterationCount;
       

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
               GetComponentsFromMyGameObject<SportsCar>();
            }
            st.Stop();
            res = st.ElapsedMilliseconds.ToString();


            st.Reset();
            st.Start();
            for (double i = 0; i < iterationCount; i++)
            {
                GetComponents<SportsCar>();
            }
            st.Stop();

            classResult = $"{res} <C U> {st.ElapsedMilliseconds}";

        }

        public void TestInterface()
        {

            string res;

            Stopwatch st = new Stopwatch();
            st.Start();
            for (double i = 0; i < iterationCount; i++)
            {
                GetComponentsFromMyGameObject<ICar>();
            }
            st.Stop();
            res = st.ElapsedMilliseconds.ToString();


            st.Reset();
            st.Start();
            for (double i = 0; i < iterationCount; i++)
            {
                GetComponents<ICar>();
            }
            st.Stop();

            intefaceResult = $"{res} <C U> {st.ElapsedMilliseconds}";

        }


    }
