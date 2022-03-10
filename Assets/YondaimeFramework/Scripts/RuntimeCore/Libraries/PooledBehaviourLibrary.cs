using System.Collections.Generic;
using System;


namespace YondaimeFramework
{


    public class PooledBehaviourLibrary : ILibrary
    {

        public void InitLibrary(Dictionary<Type, List<CustomBehaviour>> behaviourLookup, Dictionary<int, List<CustomBehaviour>> idLookup)
        {
            throw new NotImplementedException();
        }


        public void AddBehaviour<T>(T newBehaviour)
        {
            throw new NotImplementedException();
        }

        public void CleanReferencesFor(CustomBehaviour customBehaviour)
        {
            throw new NotImplementedException();
        }

        public T GetBehaviourFromLibrary<T>()
        {
            throw new NotImplementedException();
        }

        public T GetBehaviourFromLibraryById<T>(int behaviourId)
        {
            throw new NotImplementedException();
        }

        public T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
            throw new NotImplementedException();
        }

        public List<T> GetBehavioursFromLibrary<T>()
        {
            throw new NotImplementedException();
        }

        public List<T> GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId)
        {
            throw new NotImplementedException();
        }

        public T GetPooled<T>()
        {
            throw new NotImplementedException();
        }

       

        public void LogIdLookup()
        {
            throw new NotImplementedException();
        }

        public void LogLookup()
        {
            throw new NotImplementedException();
        }

        public void Pool<T>(T behaviour)
        {
            throw new NotImplementedException();
        }
    }


    public class PerformancePool<T>
    {
        public List<T> objects = new List<T>();
        
        public void Pool(T behaviour)
        {
            objects.Add(behaviour);
        }

        public T GetPooled()
        {
            int objectCount = objects.Count;

            if (objectCount <= 0)
                return default;

            objectCount--;

            T ob = objects[objectCount];
            objects.RemoveAt(objectCount);
            return ob;
        }

        
    
    }

}
