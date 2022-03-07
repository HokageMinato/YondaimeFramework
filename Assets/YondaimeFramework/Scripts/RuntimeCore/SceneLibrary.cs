using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YondaimeFramework;

namespace YondaimeFramework
{
    public class SceneLibrary : MonoBehaviour
    {
        BehaviourLibrary _behaviourLibrary = new BehaviourLibrary();
        public Car car;

        private void Start()
        {
            GenerateBehaviourLibrary();
        }

        #region RUNTIME_BEHAVIOUR_LIBRARY_HANDLES

        private void GenerateBehaviourLibrary()
        {
            _behaviourLibrary.GenerateLibrary(GetComponentsInChildren<CustomBehaviour>());
        }

        public BehaviourLibrary lib
        {
            get { 
                return _behaviourLibrary;
            }
        }

        [ContextMenu("Test")]
        void Test() 
        {
            _behaviourLibrary.AddBehaviour(Instantiate(car));
            _behaviourLibrary.LogLookup();
        }
        
        
        [ContextMenu("Test2")]
        void Test2() 
        {
            List<Car> cars = new List<Car>();
            cars.Add(Instantiate(car));
            cars.Add(Instantiate(car));
            cars.Add(Instantiate(car));
            cars.Add(Instantiate(car));

            _behaviourLibrary.AddBehaviours(cars);
            _behaviourLibrary.LogLookup();
        }
        
        [ContextMenu("Test3")]
        void Test3() 
        {
            _behaviourLibrary.LogLookup();
             Destroy(car);
            _behaviourLibrary.CleanReferencesFor<Car>();
            _behaviourLibrary.LogLookup();
        }

        #endregion

    }

  
    
}