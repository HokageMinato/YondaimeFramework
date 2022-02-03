using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "ComponentIdSource", menuName = "YondaimeFramework/ComponentIdSource")]
public class ComponentIdSources : ScriptableObject
{
    [SerializeField] private ComponentIdSource[] idSources;
    private string[] idCache;

    private HashSet<string> sourceCache = new HashSet<string>();


    public string[] GetIds()
    {
        if (IsListDirty())
        {
            GenerateNewSourceList();
            UpdateSourceCache();
        }
        return idCache;
    }


    private bool IsListDirty()
    {
        for (int i = 0; i < idSources.Length; i++)
        {
            if (idSources[i].ToBeDisplayed && !sourceCache.Contains(idSources[i].SystemId) ||
                !idSources[i].ToBeDisplayed && sourceCache.Contains(idSources[i].SystemId))
                return true;
        }
    
        return false;
    }



    private void GenerateNewSourceList()
    {
        List<string> combinedIdList = new List<string>();
        for (int i = 0; i < idSources.Length; i++)
        {
            if (idSources[i].ToBeDisplayed)
            {
                combinedIdList.AddRange(idSources[i].GetIds());
            }
        }
       
        idCache = combinedIdList.ToArray();
    }

    private void UpdateSourceCache() 
    {
        sourceCache.Clear();
        for (int i = 0; i < idSources.Length; i++)
        {
            if (idSources[i].ToBeDisplayed)
            {
                sourceCache.Add(idSources[i].SystemId);
            }
        }

    }
}

[System.Serializable]
public class ComponentIdSource
{

    [SerializeField] string _systemName;
    [SerializeField] bool _showIds;
    [SerializeField] ComponentIdSourceSO sourceSos;

    public bool ToBeDisplayed => _showIds;
    public string SystemId => _systemName;

    public string[] GetIds()
    {
        return sourceSos.GetIds();
    }

    
}