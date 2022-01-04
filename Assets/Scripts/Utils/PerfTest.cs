using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace YondaimeFramework
{
    public class PerfTest : CustomBehaviour
    {
        public DressItemView prefab;
        public CharacterItemView fab;

        private void Start()
        {
            MyLibrary.InitializeLibrary();
        }


        [ContextMenu("Gen")]
        public void GenerateObjects() {

            Transform previous = transform;
            for (int i = 0; i < 2000; i++)
            {
                previous = Instantiate(prefab, previous).transform;
            }

            Instantiate(fab, previous);
        
        }


        [ContextMenu("TF")]
        public void TestF() {

            Debug.Log("Custom GetComponentsFromChildren");

            string time;
            MyLibrary.InitializeLibrary();

            Stopwatch st = new Stopwatch();
            
            st.Start();

            time = st.Elapsed.ToString();
            st.Stop();
            st.Reset();


            GetComponentsFromLibrary<PerfTestUI>()[0]._gcfTest1.text = time;

            st.Start();
            
            time = st.Elapsed.ToString();
            st.Stop();
            st.Reset();

            GetComponentsFromLibrary<PerfTestUI>()[0]._gcfTest2.text = time;

        }

        [ContextMenu("T")]
        public void Test() {

            Debug.Log("Unity GetComponentFromChildren");

            string time;
            Stopwatch st = new Stopwatch();
            
            st.Start();
            DressItemView[] items = GetComponentsInChildren<DressItemView>();
            time = st.Elapsed.ToString();
            st.Stop();
            st.Reset();

            GetComponentsFromLibrary<PerfTestUI>()[0]._gcTest1.text = time;
            Debug.Log(time);

            st.Start();
            CharacterItemView[] itemss = GetComponentsInChildren<CharacterItemView>();
            time = st.Elapsed.ToString();
            st.Stop();
            st.Reset();

            GetComponentsFromLibrary<PerfTestUI>()[0]._gcTest2.text = time;


            Debug.Log("================================");

        }
    }
}
