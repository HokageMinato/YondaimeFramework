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



        Stopwatch st = new Stopwatch();

        st.Start();
        for (int i = 0; i < itr; i++)
        {
            MySceneLibrary.lib.GetBehavioursFromLibrary<Car>();
        }

        st.Stop();
        Debug.Log(st.ElapsedMilliseconds);


    }
    
    [ContextMenu("Test2")]
    public void Test2()
    {

        Stopwatch st = new Stopwatch();

        st.Start();

        for (int i = 0; i < itr; i++)
        {
            MySceneLibrary.lib.GetBehavioursFromLibrary2<Car>();
        }
        st.Stop();
        Debug.Log(st.ElapsedMilliseconds);


    }


}







public class SceneIDs 
{
    public const string Scene1 = "Scene1";
}