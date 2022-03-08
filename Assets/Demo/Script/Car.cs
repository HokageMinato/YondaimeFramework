using UnityEngine;
using YondaimeFramework;
using Debug = UnityEngine.Debug;

public class Car : CustomBehaviour, ICar
{

    public Car[] othercars;
    public RuntimeIdContainer carIds;
    public SportsCar sports;
    


    [ContextMenu("Test Instantiate")]
    public void Test()
    {
        ml.LogLookup();
        sports = Instantiate(sports);
        ml.LogLookup();
    }

    [ContextMenu("Test Destroy")]
    public void Test2()
    {
        ml.LogLookup();
        Destroy(sports);
        ml.LogLookup();
    }

    #region BEHAVIOUR_LIBRARY_TESTS

    [ContextMenu("Test Find")]
    void TestFind()
    {
        Debug.Log($"Class : {GetComponentFromLibrary<SportsCar>() == null} -- {gameObject.name}");
        Debug.Log($"Interface : {GetComponentFromLibrary<ICar>() == null} -- {gameObject.name}");
    }
    
    [ContextMenu("Test Find By Tag")]
    void TestFindByTag()
    {
        Debug.Log($"Class : {GetComponentFromLibraryById<SportsCar>(carIds.GetIds()[0]).gameObject.name} -- {gameObject.name}");
        Debug.Log($"Interface : {GetComponentFromLibraryById<ICar>(carIds.GetIds()[0]) == null} -- {gameObject.name}");
    }
    
    [ContextMenu("Test GetComponent")]
    void TestGetComponent()
    {
        Debug.Log($"Class : {GetComponentFromMyGameObject<SportsCar>().gameObject.name} -- {gameObject.name}");
        Debug.Log($"Interface : {GetComponentFromMyGameObject<ICar>() == null} -- {gameObject.name}");
    }
    
    [ContextMenu("Test FindObjects")]
    void TestFindObjects()
    {
        Debug.Log($"Class : {GetComponentsFromLibrary<Car>()?.Count} -- {gameObject.name}");
        Debug.Log($"Interface : {GetComponentsFromLibrary<ICar>()?.Count} -- {gameObject.name}");
    }
    

    [ContextMenu("Test GetComponents")]
    void TestGetComponents()
    {
        Debug.Log($"Class : {GetComponentsFromMyGameObject<SportsCar>()?.Count} -- {gameObject.name}");
        Debug.Log($"Interface : {GetComponentsFromMyGameObject<SportsCar>()?.Count} -- {gameObject.name}");
    }





    #endregion



}







public class SceneIDs 
{
    public const string Scene1 = "Scene1";
}