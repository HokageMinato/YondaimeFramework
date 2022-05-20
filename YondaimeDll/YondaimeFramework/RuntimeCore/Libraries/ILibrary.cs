using YondaimeFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    public interface ILibrary
    {
        public SceneId SceneId { get; }


        public T GetBehaviourFromLibrary<T>();

        public T GetBehaviourOfGameObject<T>(int requesteeGameObjectInstanceId);

        public T GetBehaviourOfGameObjectSafe<T>(int requesteeGameObjectInstanceId);

        public T GetBehaviourFromLibraryById<T>(int behaviourId);

        public IReadOnlyList<T> GetBehavioursFromLibrary<T>();

        public IReadOnlyList<T> GetBehavioursOfGameObject<T>(int requesteeGameObjectInstanceId);

        public void AddBehaviour<T>(T newBehaviour);

        public void CleanNullReferencesFor(ComponentId id,Type t);

        public T GetComponentFromOtherSceneLibrary<T>(string sceneId);

        public T GetComponentFromOtherSceneLibraryById<T>(ComponentId behaviourId, string sceneId);

        public IReadOnlyList<T> GetComponentsFromOtherSceneLibrary<T>(string sceneId);

        public T GetPooled<T>();

        public void Pool(CustomBehaviour behaviour);

        public void LogIdLookup();

        public void LogLookup();

        public void LogGOLookup();

        public void SetBehaviours(CustomBehaviour[] behv);

        public void SetComponentId(CustomBehaviour behaviour,ComponentId newId);

    }
}
