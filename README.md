Yondaime Framework
Performant component-fetch framework

Benchmarks:

<C are framework methods, U> are unity counterparts.

Top row indicates class instances fetch performance

Bottom row indicates interface instances fetch performance

N/A stands for unavailability of method

![alt text](https://i.ibb.co/gJ37x9J/Test.jpg)


GOALS:

A ﬂexible performant alternative to and built upon
existing unity component system.

Next TODO:

- Add collection Marshalling and utilize System.Memory.
- Convert to Extension Methods based workflow instead of CustomBehaviour Extension.

Why?:

● Performant access to components with global scope and Inspector hierarchal independence.
● Destroy need for singleton.


Auto-managed interscene communication.

Extras:
● public T GetComponentFromOtherSceneLibrary<T>(string sceneId); //SceneId to identify which scene from

● public T GetComponentFromOtherSceneLibraryById<T>(ComponentId behaviourId,string sceneId) //To get exact beh

● public List<T> GetComponentsFromOtherSceneLibrary<T>(string sceneId)
