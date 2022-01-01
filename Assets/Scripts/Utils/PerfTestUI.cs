using UnityEngine;
using UnityEngine.UI;


namespace YondaimeFramework
{
    public class PerfTestUI : CustomBehaviour
    {

        public Text _gcTest1 ,_gcTest2;
        public Text _gcfTest1,_gcfTest2;

        public void OnGCTest() {
            GetComponentsFromLibrary<PerfTest>()[0].Test();
        }


        public void OnGCFTest() {
            GetComponentsFromLibrary<PerfTest>()[0].TestF();
        }

    }
}