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

        [ContextMenu("Assign")]
        public void AssignId() 
        {
            int c = 1;
            for (int i = 0; i < idsData.Length; i++)
            {
                var data = idsData[i];
                var idSRCData = data.GetIdSRCs();
                var idData = data.GetIds();
                idData = new ComponentId[idSRCData.Length];

                for (int j = 0; j < idSRCData.Length; j++)
                {
                    idSRCData[j].intValue = c;
                    idData[j] = new ComponentId(idSRCData[j]);
                    c++;
                }

                data.SetParsedValues(idData);
                data.SetDirty();
            }
            #if UNITY_EDITOR
             EditorUtility.SetDirty(this);
            #endif
        }

    }

    [System.Serializable]
    public class ComponentId 
    {
        public const int None = 0;
        public int objBt;
        public int _goInsId;
        //Make sure these are same or editor will throw out errors


        //#if UNITY_EDITOR
        public string stringId;
        public const string StringIdPropertyName = "stringId";
        public const string IntIdValName = "objBt";
        public const string NoneStr = "None";
       
        public ComponentId(ComponentIdSRC source) 
        {
            stringId = source.stringIdVal;
            objBt = source.intValue;
        }

        public ComponentId() { }
        //#endif

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

       
        public ComponentIdSRC[] GetIdSRCs()
        {
            return sourceSos.GetIdSRCs();
        }


        public ComponentId[] GetIds() 
        {
            return sourceSos.GetIds();
        }


        public void SetParsedValues(ComponentId[] parsedIds) 
        {
            sourceSos.SetParsedValues(parsedIds);
        }
      
        public void SetDirty()
        {
            #if UNITY_EDITOR
            EditorUtility.SetDirty(sourceSos);
            #endif
        }


    }
}