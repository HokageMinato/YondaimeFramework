using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Tag
{
    public class PerfTest : CustomBehaviour
    {
        public DressItemView prefab;
        public CharacterItemView fab;

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

            string time;
            Stopwatch st = new Stopwatch();
            
            st.Start();
            CustomBehaviour[] items =  GetComponentsInChildrenFast<DressItemView>();
            time = st.Elapsed.ToString();
            st.Stop();
            st.Reset();
            Debug.Log(time);

            st.Start();
            CustomBehaviour[] itemss = GetComponentsInChildrenFast<CharacterItemView>();
            time = st.Elapsed.ToString();
            st.Stop();
            st.Reset();

            Debug.Log(time);

        }

        [ContextMenu("T")]
        public void Test() {

            string time;
            Stopwatch st = new Stopwatch();
            
            st.Start();
            DressItemView[] items = GetComponentsInChildren<DressItemView>();
            time = st.Elapsed.ToString();
            st.Stop();
            st.Reset();

            Debug.Log(time);

            st.Start();
            CharacterItemView[] itemss = GetComponentsInChildren<CharacterItemView>();
            time = st.Elapsed.ToString();
            st.Stop();
            st.Reset();

            Debug.Log(time);

            

        }
    }
}
