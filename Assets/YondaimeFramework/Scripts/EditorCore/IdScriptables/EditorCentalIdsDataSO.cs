using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

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



        public void AssignId()
        {
            SearchForDuplicates();

            for (int i = 0; i < idsData.Length; i++)
            {
                var data = idsData[i];
                var idSRCData = data.GetIdSRCs();

                for (int j = 0; j < idSRCData.Length; j++)
                {
                    var strId = idSRCData[j].stringIdVal;
                    strId = strId.Trim();

                    idSRCData[j].intValue = GetDeterministicHash(strId);
                    idSRCData[j].stringIdVal = (strId);
                }


                data.UpdateRuntimeIdValues();
                data.SetDirty();
            }

            SearchForDuplicateHash();
            
           
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
                        
                        idsData[i].GetIdSRCs()[j].stringIdVal = string.Empty;
                        idsData[i].GetIdSRCs()[j].intValue = 0;
                        throw new Exception($"Duplicate Id entry ({stringIdValue}) found at {idsData[i].SystemId}, Fixing");
                    }

                    if(stringIdValue != string.Empty)
                    idv.Add(stringIdValue);
                }                

            }
        }  
        
        
        private void SearchForDuplicateHash()
        {
            HashSet<int> idv = new HashSet<int>();

            for (int i = 0; i < idsData.Length; i++)
            {
               
                for (int j = 0; j < idsData[i].GetIdSRCs().Length; j++)
                {
                    int intIdValue = idsData[i].GetIdSRCs()[j].intValue;
                    string stringIdValue = idsData[i].GetIdSRCs()[j].stringIdVal;

                    if (idv.Contains(intIdValue))
                    {
                        throw new Exception($"Hash of id:{stringIdValue} at {idsData[i].SystemId} generates a collision, This is a framework issue and will be solved in future. To overcome this you will have to" +
                                            $"change the id:{stringIdValue} to something else.");
                    }

                    if(intIdValue!=0)
                    idv.Add(intIdValue);
                }                

            }
        }


        private int GetDeterministicHash(string stringIdVal)
        {
            return DeterministicHashGenerator.GetHashOf(stringIdVal);
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