using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;

namespace YondaimeFramework
{
    public class SceneLibrary : MonoBehaviour
    {

        public List<BehaviourLibrary> _libraries;
        public List<CustomBehaviour> _behaviours;

        
        public void ScanBehaviours()
        {

            _libraries.Clear();
            _behaviours.Clear();

            List<BehaviourLibrary> libraries = new List<BehaviourLibrary>(GetComponentsInChildren<BehaviourLibrary>());
            List<CustomBehaviour> behaviours = new List<CustomBehaviour>(GetComponentsInChildren<CustomBehaviour>());

            PooledLibrary[] poolLibraries = GetComponentsInChildren<PooledLibrary>();

            SetTagToComponentLibraries();
            SeperateBehaviours();
            SeperateLibraries();
            //SetActiveLibraries();
            
            
            void SetTagToComponentLibraries() 
            {
                for (int i = 0; i < libraries.Count; i++)
                {
                    libraries[i].SetTag();
                }
                
            }

            void SeperateBehaviours()
            {

                for (int i = 0; i < behaviours.Count; i++)
                {
                    CustomBehaviour customBehaviour = behaviours[i];
                    BehaviourLibrary parentLibrary = GetLibraryParentOf(customBehaviour);
                    if (parentLibrary != null)
                    {
                        customBehaviour.SetLibrary(parentLibrary);
                        parentLibrary.AddBehaviour(customBehaviour);
                    }
                    else 
                    {
                        _behaviours.Add(customBehaviour);
                    }

                    

                }
                
            }


            void SeperateLibraries()
            {
                for (int i = 0; i < libraries.Count;i++)
                {
                    BehaviourLibrary behaviourLibrary = libraries[i];
                    BehaviourLibrary parentLibrary = GetLibraryParentOfLib(behaviourLibrary);


                    if (parentLibrary)
                    {
                        parentLibrary.AddLibrary(behaviourLibrary);
                    }
                    else 
                    {
                        _libraries.Add(behaviourLibrary);
                    }
                }
                
            }
               


            BehaviourLibrary GetLibraryParentOf(CustomBehaviour behaviour)
            {
                Transform t = behaviour.transform;

                while (t != null && !t.CompareTag(BehaviourLibrary.COMPONENT_TAG))
                {
                    t = t.parent;
                }

                if (t == null)
                    return null;

                for (int i = 0; i < libraries.Count; i++)
                {
                    BehaviourLibrary lib = libraries[i];
                    if (lib.transform == t)
                        return lib;
                }

                return null;
            }

            BehaviourLibrary GetLibraryParentOfLib(BehaviourLibrary otherLibrary)
            {

                Transform t = otherLibrary.transform.parent;

                while (t != null && t != otherLibrary.transform && !t.CompareTag(BehaviourLibrary.COMPONENT_TAG))
                {
                    
                    t = t.parent;
                }


                if (t == null)
                    return null;

                for (int i = 0; i < libraries.Count; i++)
                {
                    BehaviourLibrary lib = libraries[i];
                    if (lib.transform == t)
                        return lib;
                }

                return null;

            }

        }
    }
}