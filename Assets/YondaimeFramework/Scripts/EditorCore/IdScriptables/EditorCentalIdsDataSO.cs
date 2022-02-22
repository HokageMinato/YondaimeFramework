using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

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
            SearchForDuplicates();

            for (int i = 0; i < idsData.Length; i++)
            {
                var data = idsData[i];
                var idSRCData = data.GetIdSRCs();

                for (int j = 0; j < idSRCData.Length; j++)
                {
                    idSRCData[j].intValue = Animator.StringToHash(idSRCData[j].stringIdVal);
                }

                data.UpdateRuntimeIdValues();
                data.SetDirty();
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        private void SearchForDuplicates()
        {
            HashSet<string> idv = new HashSet<string>();

            for (int i = 0; i < idsData.Length; i++)
            {
               
                for (int j = 0; j < idsData[i].GetIdSRCs().Length; j++)
                {
                    string stringIdValue = idsData[i].GetIdSRCs()[j].stringIdVal;

                    if (idv.Contains(stringIdValue))
                    {
                        throw new Exception($"Duplicate Id entry ({stringIdValue}) found at {idsData[i].SystemId}");
                    }

                    idv.Add(stringIdValue);
                }                

            }
        }
    }

    

    
    [System.Serializable]
    public class ComponentIdSRC 
    {
        public string stringIdVal;
        [HideInInspector]public int intValue;

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