using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;
using System;
using System.Diagnostics;

namespace Tag
{
    public class GetComponentLibVsFindObject : CustomBehaviour, IProfileComponent
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
                    GetComponentFromLibrary<SportsCar>();
                }
                st.Stop();
                res = st.ElapsedMilliseconds.ToString();


                st.Reset();
                st.Start();
                for (double i = 0; i < iterationCount; i++)
                {
                    FindObjectOfType<SportsCar>();
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
               GetComponentFromLibrary<ICar>();
            }
            
            st.Stop();
            res = st.ElapsedMilliseconds.ToString();

            intefaceResult = ($"{res} <C U> N/A");


           
        }
    }
}
