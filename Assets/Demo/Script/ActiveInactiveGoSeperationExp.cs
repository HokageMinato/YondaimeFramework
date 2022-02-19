using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;
using System;

namespace Tag
{
    public class ActiveInactiveGoSeperationExp : CustomBehaviour
    {
        
        public Dictionary<Type,StateSeperatedList<CustomBehaviour>> pooledLookup = new Dictionary<Type, StateSeperatedList<CustomBehaviour>>();



    }


    public class StateSeperatedList<T>
    {

        public List<T> activeObjects = new List<T>();
        public List<T> inActiveObjects = new List<T>();

        public int ActiveCount
        {
            get 
            { 
                return activeObjects.Count; 
            }
        }
        public int InActiveCount
        {
            get {
                return inActiveObjects.Count;
            }
        }

        public int Count 
        { 
            get
            { 
                return activeObjects.Count + inActiveObjects.Count - 1;
            }
        }


        public void Add(T newObject,bool isActive) {

           
            if (isActive)
            {
                activeObjects.Add(newObject);
            }
            else {
                inActiveObjects.Add(newObject);
            
            }
        }

        public T Remove(bool requireActive) 
        {
            if (requireActive)
            {
                T obj = activeObjects[activeObjects.Count - 1];
                activeObjects.RemoveAt(activeObjects.Count -1);
                return obj;
                
            }
            else
            {
                T obj = inActiveObjects[inActiveObjects.Count - 1];
                inActiveObjects.RemoveAt(inActiveObjects.Count - 1);
                return obj;

            }
        }
        
       
    }

}
