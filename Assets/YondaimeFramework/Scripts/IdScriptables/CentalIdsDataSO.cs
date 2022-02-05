using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YondaimeFramework
{
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
    public struct ComponentId 
    {
        public string stringId;
        public int objBt;

        //Make sure these are same or editor will throw out errors
        public const string StringIdPropertyName = "stringId";
        public const string IntIdValName = "objBt";
        public const int None = -1;
        public const string NoneStr = "None";

        
        public ComponentId(ComponentIdSRC source) 
        {
            stringId = source.stringIdVal;
            objBt = source.intValue;
        }

    }

    [System.Serializable]
    public struct ComponentIdSRC 
    {
        public string stringIdVal;
        public int intValue;

    }


    [System.Serializable]
    public class SystemIdsData
    {

        [SerializeField] string _systemName;
        [SerializeField] SystemIdDataSO sourceSos;

        public string SystemId => _systemName;
        public ComponentIdSRC[] GetIds()
        {
            return sourceSos.GetIds();
        }


    }
}