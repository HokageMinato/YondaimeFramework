using YondaimeFramework;
using System;
using System.Collections.Generic;

namespace YondaimeFramework
{
    public interface ILibrary
    {
        public void InitLibrary(Dictionary<Type, List<CustomBehaviour>> behaviourLookup,
                                Dictionary<int, List<CustomBehaviour>> idLookup);

        public T GetBehaviourFromLibrary<T>();

        public T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId);

        public T GetBehaviourFromLibraryById<T>(int behaviourId);

        public List<T> GetBehavioursFromLibrary<T>();

        public List<T> GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId);

        public void AddBehaviour<T>(T newBehaviour);

        public void CleanReferencesFor(CustomBehaviour customBehaviour);

        public void AddBehaviours<T>(List<T> customBehaviour);

        public void AddBehaviours<T>(T[] customBehaviour);

        public void LogIdLookup();

        public void LogLookup();

    }
}
