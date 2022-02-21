using UnityEngine;
using UnityEditor;


namespace YondaimeFramework.EditorHandles
{
    public class EditorCentalIdsDataSO : ScriptableObject
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
                
                for (int j = 0; j < idSRCData.Length; j++)
                {
                    idSRCData[j].intValue = c;
                    c++;
                }

                data.UpdateRuntimeIdValues();
                data.SetDirty();
            }
            
            EditorUtility.SetDirty(this);
            
        }

    }

    

    
    [System.Serializable]
    public class ComponentIdSRC 
    {
        public string stringIdVal;
        public int intValue;

        public ComponentId ParseToRuntimeId() 
        {
            return new ComponentId()
            {
                stringId = stringIdVal,
                objBt = intValue
            };
        }
    }
    




    [System.Serializable]
    public class SystemIdsData
    {

        [SerializeField] string _systemName;
        [SerializeField] EditorIdContainer sourceSos;
        public string SystemId => _systemName;

       
        public ComponentIdSRC[] GetIdSRCs()
        {
            return sourceSos.GetIdSRCs();
        }

        public void UpdateRuntimeIdValues() 
        {
            sourceSos.UpdateRuntimeValues();
        }

        public void SetDirty()
        {
            EditorUtility.SetDirty(sourceSos);
        }


    }
}