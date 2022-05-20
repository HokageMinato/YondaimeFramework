using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

namespace YondaimeFramework.EditorHandles
{
    public class EditorCentalIdsDataSO : ScriptableObject
    {
        
        [SerializeField] private List<SystemIdsData> idsData;
        public List<SystemIdsData> SystemIdsData
        {
            get
            {
                return idsData;
            }
        }

        [ContextMenu("Check for Duplicates")]
        public void SearchForDuplicates()
        {
            SearchForDuplicateScriptables();
            SearchForDuplicateIds();
            SearchForDuplicateHash();
        }


        public void AssignId()
        {
            for (int i = 0; i < idsData.Count; i++)
            {
                var data = idsData[i];
                if (data.Container == null)
                    return;

                var idSRCData = data.GetIdSRCs();

                for (int j = 0; j < idSRCData.Count; j++)
                {
                    var strId = idSRCData[j].stringIdVal;
                    strId = strId.Trim();

                    if(data.hashAlgorithm == EditorHandles.SystemIdsData.HashAlgorithm.DETERMINISTIC)
                        idSRCData[j].intValue = GetDeterministicHash(strId);
                    else if (data.hashAlgorithm == EditorHandles.SystemIdsData.HashAlgorithm.ANIMATION)
                        idSRCData[j].intValue = GetAnimatorGeneratedHash(strId);


                    idSRCData[j].stringIdVal = strId;
                }


                data.UpdateRuntimeIdValues();
                data.SetDirty();
            }

        }



       

        private void SearchForDuplicateScriptables() 
        {
            HashSet<EditorIdContainer> container = new HashSet<EditorIdContainer>();
            for (int i = 0; i < idsData.Count;)
            {
                EditorIdContainer id = idsData[i].Container;
                if (!container.Contains(id))
                {
                    container.Add(id);
                    i++;
                }
                else
                {
                    Debug.LogError($"Duplicate Scriptable({id.name}) found , Deleting Duplicate");
                    idsData.RemoveAt(i);
                }
            }
        }

        private void SearchForDuplicateIds()
        {
            HashSet<string> idv = new HashSet<string>();

            for (int i = 0; i < idsData.Count; i++)
            {
                SystemIdsData systemIdsData = idsData[i];

                if (systemIdsData.Container == null)
                    break;

                if (systemIdsData.hashAlgorithm == EditorHandles.SystemIdsData.HashAlgorithm.ANIMATION)
                    break;

                List<ComponentIdSRC> idSources = systemIdsData.GetIdSRCs();

                for (int j = 0; j < idSources.Count;)
                {
                    string stringIdValue = idSources[j].stringIdVal;

                    if (idv.Contains(stringIdValue))
                    {
                        idSources.RemoveAt(j);
                        Debug.LogError($"Duplicate Id entry ({stringIdValue}) found at {systemIdsData.SystemId}, Deleting Duplicate");
                        continue;
                    }

                    if(stringIdValue != string.Empty)
                    idv.Add(stringIdValue);
                    j++;
                }                

            }
        }  
        
        
        private void SearchForDuplicateHash()
        {
            HashSet<int> idv = new HashSet<int>();

            for (int i = 0; i < idsData.Count; i++)
            {
                if (idsData[i].Container == null)
                    return;

                if (idsData[i].hashAlgorithm == EditorHandles.SystemIdsData.HashAlgorithm.ANIMATION)
                    return;

                for (int j = 0; j < idsData[i].GetIdSRCs().Count; j++)
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

        private int GetAnimatorGeneratedHash(string stringIdVal) 
        { 
            return Animator.StringToHash(stringIdVal);
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
        [SerializeField] private HashAlgorithm _hashAlgorithm;
        public string SystemId => _systemName;
        public HashAlgorithm hashAlgorithm => _hashAlgorithm;
        public EditorIdContainer Container
        {
            get
            {
                return sourceSos;
            }
            set 
            { 
                sourceSos = value;
            }
        }

        public List<ComponentIdSRC> GetIdSRCs()
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

        public enum HashAlgorithm
        {
            DETERMINISTIC,
            ANIMATION
        }
    }



    

}