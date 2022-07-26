using UnityEngine;


[CreateAssetMenu(fileName = "ExecutionSettings", menuName = "YondaimeFramework/ExecutionSettings")]
public class ExecutionModeSO : ScriptableObject
{
    public XecutionMode ExecutionMode;


    public enum XecutionMode 
    { 
        EDITOR_DEBUG = 0,
        SIMULATED =1
    }
}
