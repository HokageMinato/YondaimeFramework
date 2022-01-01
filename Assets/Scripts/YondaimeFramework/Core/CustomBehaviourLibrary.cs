using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tag
{
    public class CustomBehaviourLibrary : CustomBehaviour
    {
        public CustomBehaviour[] behaviours;
        public CustomBehaviourLibrary[] childLibs;
        private Dictionary<Type, List<CustomBehaviour>> behaviourLookUp= new Dictionary<Type, List<CustomBehaviour>>();
        
        public void InitializeLookUp()
        {
            GenerateLookUp();
            FillLookUp();
            InitChildLibraries();

            void GenerateLookUp()
            {
                Debug.Log(gameObject.name);
                for (int i = 0; i < behaviours.Length; i++)
                {
                    Type behaviourType = behaviours[i].GetType();
                    if (!behaviourLookUp.ContainsKey(behaviourType))
                    {
                        behaviourLookUp.Add(behaviourType, new List<CustomBehaviour>());
                    }
                }
            }
            void FillLookUp()
            {
                for (int i = 0; i < behaviours.Length; i++)
                {
                    behaviourLookUp[behaviours[i].GetType()].Add(behaviours[i]);
                }
            }

            void InitChildLibraries() {
                for (int i = 0; i < childLibs.Length; i++)
                {
                    childLibs[i].InitializeLookUp();
                }
            }
        }

        public CustomBehaviour[] GetBehaviours<I>()
        {
            Type reqeuestedType = typeof(I);

            List<CustomBehaviour> behaviours = new List<CustomBehaviour>();

            if (behaviourLookUp.ContainsKey(reqeuestedType))
                return behaviourLookUp[reqeuestedType].ToArray();

            foreach (KeyValuePair<Type, List<CustomBehaviour>> item in behaviourLookUp)
            {
                
                if (typeof(I).IsAssignableFrom(item.Key))
                {
                    behaviours.AddRange(item.Value);
                }
            }

            return behaviours.ToArray();

        }

        public CustomBehaviour[] GetBehavioursInChildren<I>()
        {
            List<CustomBehaviour> behaviours = new List<CustomBehaviour>();
            behaviours.AddRange(GetBehaviours<I>());
            for (int i = 0; i < childLibs.Length; i++)
                behaviours.AddRange(childLibs[i].GetBehaviours<I>());

            

            return behaviours.ToArray();
        }

     
        public CustomBehaviour GetBehaviour<I>()
        {
            Debug.Log($"Requested " + typeof(I));

            
            foreach (var item in behaviourLookUp)
            {
                Debug.Log("HAS"+item.Key.ToString());
            }

            if(behaviourLookUp[typeof(I)].Count > 0)
                return behaviourLookUp[typeof(I)][0];
              return null;
        }
       


        [ContextMenu("Scan")]
        public void ScanTypes()
        {
            List<CustomBehaviour> scannedBehaviour = new List<CustomBehaviour>(GetComponentsInChildren<CustomBehaviour>(true));
            List<CustomBehaviourLibrary> childLibraries = new List<CustomBehaviourLibrary>();
            RemoveRedundantBehaviours();
            AssignScannedBehaviours();
            SetSelfAsActiveLibraryToBehaviours();
            childLibs = childLibraries.ToArray();
          

            void RemoveRedundantBehaviours()
            {
                for (int i = 0; i < scannedBehaviour.Count; i++)
                {
                    if (IsAChildLibrary(scannedBehaviour[i]))
                    {
                        CustomBehaviourLibrary foundLibrary = scannedBehaviour[i] as CustomBehaviourLibrary;
                        FilterScannedBehavioursByLibrary(foundLibrary);
                        AddToChildLibrares(foundLibrary);
                    }
                }
            }
            void SetSelfAsActiveLibraryToBehaviours()
            {
                for (int i = 0; i < behaviours.Length; i++)
                {
                    behaviours[i].SetLibrary(this);
                }
            }
            void AssignScannedBehaviours() {
                behaviours = scannedBehaviour.ToArray();
            }
            bool IsAChildLibrary(CustomBehaviour behaviour)
            {
                return behaviour.GetType().Equals(typeof(CustomBehaviourLibrary)) &&
                      !behaviour.Equals(this);
            }
            void FilterScannedBehavioursByLibrary(CustomBehaviourLibrary library) {

                for (int i = 0; i < library.behaviours.Length; i++)
                {
                    scannedBehaviour.Remove(library.behaviours[i]);
                }
            }
            void AddToChildLibrares(CustomBehaviourLibrary library) {
                library.SetLibrary(this);
                childLibraries.Add(library);
            }

        }

        

    }
}
