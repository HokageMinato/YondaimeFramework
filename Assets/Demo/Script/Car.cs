using UnityEngine;
using YondaimeFramework;
using Debug = UnityEngine.Debug;
using System.Diagnostics;
using System.Collections.Generic;

public class Car : CustomBehaviour, ICar
{

    public Car[] othercars;
    public RuntimeIdContainer carIds;
    private const string tid = "Finish";
    public int itr;
    public Transform other;

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

    }


    [ContextMenu("Test")]
    public void Test()
    {
        Instantiate(this);
    }

    [ContextMenu("Test2")]
    public void Test2()
    {

        //Stopwatch st = new Stopwatch();

        //st.Start();

        //for (int i = 0; i < itr; i++)
        //{
        //    RefreshHierarchyOld();
        //}
        //st.Stop();
        //Debug.Log(st.ElapsedMilliseconds);


    }

    #region BEHAVIOUR_LIBRARY_TESTS

    //[ContextMenu("Test")]
    //void Test()
    //{
    //    _behaviourLibrary.AddBehaviour(Instantiate(car));
    //    _behaviourLibrary.LogLookup();
    //}


    //[ContextMenu("Test2")]
    //void Test2()
    //{
    //    List<Car> cars = new List<Car>();
    //    cars.Add(Instantiate(car));
    //    cars.Add(Instantiate(car));
    //    cars.Add(Instantiate(car));
    //    cars.Add(Instantiate(car));

    //    _behaviourLibrary.AddBehaviours(cars);
    //    _behaviourLibrary.LogLookup();
    //}

    //[ContextMenu("Test3")]
    //void Test3()
    //{
    //    _behaviourLibrary.LogLookup();
    //    Destroy(car);
    //    _behaviourLibrary.CleanReferencesFor<Car>();
    //    _behaviourLibrary.LogLookup();
    //}

    #endregion



}







public class SceneIDs 
{
    public const string Scene1 = "Scene1";
}