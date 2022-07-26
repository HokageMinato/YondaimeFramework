using System;
using UnityEditor;
using UnityEngine;

namespace YondaimeFramework.EditorHandles{

    [InitializeOnLoad]
    public class SceneDirtyDetector : CustomBehaviour
    {
        private static ILibrary targetLibrary;
        private static ExecutionModeSO execModeSO;

        static SceneDirtyDetector()
        {
            EditorApplication.update += InvokeBehavioursScan;
        }

        private static void InvokeBehavioursScan()
        {
            if (Application.isPlaying && IsExecutionSetToSimulatedMode())
                return;
            
            if (targetLibrary == null)
               ScanForLibrary();
            

            (targetLibrary).SetBehaviours(FindObjectsOfType<CustomBehaviour>(true));
        }

        private static bool IsExecutionSetToSimulatedMode()
        {
            if(execModeSO == null)
                execModeSO = AssetDatabase.LoadAssetAtPath<ExecutionModeSO>(ASSET_PATHS.ExecutionSettings);

            return execModeSO.ExecutionMode == ExecutionModeSO.XecutionMode.SIMULATED;
        }

        private static void ScanForLibrary() 
        {

            MonoBehaviour[] behvs = FindObjectsOfType<MonoBehaviour>();
            for (int i = 0; i < behvs.Length; i++) 
            {
                ILibrary library = behvs[i].GetComponent<ILibrary>();
                if (library != null)
                    targetLibrary = library;
            }
        }

        public static void Refresh() 
        {
            targetLibrary = null;
        }

        void LoadIdSource()
        {
            execModeSO = AssetDatabase.LoadAssetAtPath<ExecutionModeSO>(ASSET_PATHS.CentalIdContainerAssetPath);
        }

    }

}