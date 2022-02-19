using UnityEngine;
using YondaimeFramework;
using Debug = UnityEngine.Debug;
using System.Collections.Generic;

public class Car : CustomBehaviour, ICar
{

    public Car[] othercars;
    public RuntimeIdContainer carIds;
    private const string tid = "Finish";
    public int itr;

    private void Start()
    {
        //Debug.Log("===============================");
        //Debug.Log($"{GetComponentFromLibrary<SportsCar>() == null} -- {gameObject.name}");
        //Debug.Log($"{GetComponentFromLibraryById<SportsCar>(idToSearch)?.gameObject.name} -- {gameObject.name}");
        //Debug.Log($"{GetComponentFromLibraryById<ICar>(idToSearch)==null} -- {gameObject.name}");
        //Debug.Log($"{GetComponentFromLibraryById<Car>(idToSearch)==null} -- {gameObject.name}");
        //Debug.Log($"{GetComponentFromLibraryById<Truck>(idToSearch)==null} -- {gameObject.name}");
        //Debug.Log($"{GetComponentFromMyGameObject<SportsCar>() == null} -- {gameObject.name}");
        //Debug.Log($"{GetComponentsFromLibrary<Car>()?.Count} -- {gameObject.name}" );
        //Debug.Log($"{GetComponentsFromMyGameObject<SportsCar>()?.Count} -- {gameObject.name}");
        //Debug.Log("===============================");

        Debug.Log(carIds.GetIds() == null);
    }


    [ContextMenu("Test")]
    public void Test()
    {

        List<int> list = new List<int>(10);
        Debug.Log(list.Capacity);

        

       for (int i = 0; i < 21; i++)
        {
            list.Add(i);

        }
        Debug.Log(list.Capacity);
        //Debug.Log(list[4]);


    }


}







public class SceneIDs 
{
    public const string Scene1 = "Scene1";
}