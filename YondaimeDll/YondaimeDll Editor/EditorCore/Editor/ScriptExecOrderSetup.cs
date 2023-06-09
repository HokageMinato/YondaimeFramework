using UnityEditor;
using UnityEngine;
using YondaimeFramework;


namespace YondaimeFramework.EditorHandles
{
    [InitializeOnLoad]
    public class ScriptExecOrderSetup : CustomBehaviour
    {
        const string INIT_STATE = "FirstInitDone";
        
        static ScriptExecOrderSetup()
        {
            if (!IsOrderSetForThisSession())
            {
                SetExecOrderOf(GetMonoSciptOf("RootLibrary"), -50);
                SetExecOrderOf(GetMonoSciptOf("StandardBehaviourLibrary"), -21);
                SetExecOrderOf(GetMonoSciptOf("PooledBehaviourLibrary"), -20);
               
                SetOrderSetForThisSession();
            }
        }

        static bool IsOrderSetForThisSession() 
        { 
            return SessionState.GetBool(INIT_STATE,false);
        }

        static void SetOrderSetForThisSession() 
        {
            SessionState.SetBool(INIT_STATE, true);
        }

        static MonoScript GetMonoSciptOf(string className) 
        { 
           MonoScript[] scripts = MonoImporter.GetAllRuntimeMonoScripts();
           return ArrayUtility.Find(scripts,x=> x.name == className);
        }

        static void SetExecOrderOf(MonoScript monoScript, int execOrder) 
        {
            MonoImporter.SetExecutionOrder(monoScript, execOrder);
        }

    }
}
