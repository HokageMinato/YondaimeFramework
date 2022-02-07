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
            int c = 0;
            for (int i = 0; i < idsData.Length; i++)
            {
                var data = idsData[i];
                var idData = data.GetIds();

                for (int j = 0; j < idData.Length; j++)
                {
                    idData[j].intValue = c;
                    c++;
                }
                data.SetDirty();
            }
            #if UNITY_EDITOR
             EditorUtility.SetDirty(this);
            #endif
        }

    }

    [System.Serializable]
    public struct ComponentId 
    {
        #if UNITY_EDITOR
        public string stringId;
        public const string StringIdPropertyName = "stringId";
        public const string IntIdValName = "objBt";
        public const int None = -1;
        public const string NoneStr = "None";
        #endif


        public int objBt;
        //Make sure these are same or editor will throw out errors

        #if UNITY_EDITOR
        public ComponentId(ComponentIdSRC source) 
        {
            stringId = source.stringIdVal;
            objBt = source.intValue;
        }
       #endif

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

      
        public void SetDirty()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(sourceSos);
#endif
        }


    }
}