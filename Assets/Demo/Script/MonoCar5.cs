using UnityEngine;
using YondaimeFramework;

    public class MonoCar5 : MonoBehaviour
    {

    #region BEHAVIOUR_LIBRARY_TESTS
    public SceneId sceneIdToGetFrom;
    public ComponentId idToGet;
    public Car otherObj;

    

    [ContextMenu("Test Find By Tag")]
    void TestFindByTag()
    {

        Debug.Log($"Class : {CustomBehaviour.FindObjFromLibraryById<SportsCar>(idToGet) == null}");
        Debug.Log($"Interface : {CustomBehaviour.FindObjFromLibraryById<ICar>(idToGet) == null} ");

    }

    [ContextMenu("Test GetComponent")]
    void TestGetComponent()
    {

        Debug.Log($"Class :  {CustomBehaviour.GetComponentFromGameObjectOf<SportsCar>(otherObj) == null}");
        Debug.Log($"Interface : {CustomBehaviour.GetComponentFromGameObjectOf<ICar>(otherObj) == null}");
    }

    [ContextMenu("Test GetComponents")]
    void TestGetComponents()
    {
        Debug.Log($"Class :  {CustomBehaviour.GetComponentsFromGameObjectOf<SportsCar>(otherObj) == null}");
        Debug.Log($"Interface : {CustomBehaviour.GetComponentsFromGameObjectOf<ICar>(otherObj) == null}");
    }

    [ContextMenu("Test Find Obj")]
    void TestFind()
    {
        Debug.Log($"Class : {CustomBehaviour.FindObjFromLibrary<SportsCar>() == null}");
        Debug.Log($"Interface : {CustomBehaviour.FindObjFromLibrary<ICar>() == null}");
    }

    [ContextMenu("Test Find Objs")]
    void TestFindObjs()
    {
        Debug.Log($"Class : {CustomBehaviour.FindObjsFromLibrary<Car>()?.Count}");
        Debug.Log($"Interface : {CustomBehaviour.FindObjsFromLibrary<ICar>()?.Count}");
    }


    

    

    #endregion



}
