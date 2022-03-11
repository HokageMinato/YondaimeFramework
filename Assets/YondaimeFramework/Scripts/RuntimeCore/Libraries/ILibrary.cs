using YondaimeFramework;
using System;
using System.Collections.Generic;

namespace YondaimeFramework
{
    public interface ILibrary
    {
        public SceneId SceneId { get; }

        public T GetBehaviourFromLibrary<T>();

        public T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId);

        public T GetBehaviourFromLibraryById<T>(int behaviourId);

        public List<T> GetBehavioursFromLibrary<T>();

        public List<T> GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId);

        public void AddBehaviour<T>(T newBehaviour);

        public void CleanReferencesFor(CustomBehaviour customBehaviour);

        public T GetComponentFromOtherSceneLibrary<T>(string sceneId);

        public T GetComponentFromOtherSceneLibraryById<T>(ComponentId behaviourId, string sceneId);

        public List<T> GetComponentsFromOtherSceneLibrary<T>(string sceneId);

        public T GetPooled<T>();

        public void Pool<T>(T behaviour);

        public void LogIdLookup();

        public void LogLookup();

        public void SetBehaviours(CustomBehaviour[] behv);

    }
}
