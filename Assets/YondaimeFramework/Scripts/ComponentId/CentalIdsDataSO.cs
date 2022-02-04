using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "ComponentIdSource", menuName = "YondaimeFramework/ComponentIdSource")]
public class CentalIdsDataSO : ScriptableObject
{
    [SerializeField] private SystemIdsData[] idsData;
   
    public SystemIdsData[] SystemIdsData
    {
        get 
        {
            return idsData;   
        }
    }
     
}

[System.Serializable]
public class SystemIdsData
{

    [SerializeField] string _systemName;
    [SerializeField] SystemIdDataSO sourceSos;

    public string SystemId => _systemName;
    public string[] GetIds()
    {
        return sourceSos.GetIds();
    }

    
}