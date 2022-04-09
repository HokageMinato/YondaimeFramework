using System;
using UnityEditor;
using UnityEngine;

namespace YondaimeFramework.EditorHandles
{

    [InitializeOnLoad]
    public class CentralIdDataValidator : CustomBehaviour
    {

        static float t;
        private const float triggerFrequency = 1 * 1000;

        static CentralIdDataValidator()
        {
            EditorApplication.update += () =>
            {
                ScanPerSecond(InvokeValidation);
            };
        }




        private static void ScanPerSecond(Action invokeAction)
        {
            t += Time.time;

            if (t >= triggerFrequency)
            {
                invokeAction();
                t = 0;
            }
        }

        private static void InvokeValidation()
        {
            EditorCentalIdsDataSO centralIdContainer = AssetDatabase.LoadAssetAtPath<EditorCentalIdsDataSO>(ASSET_PATHS.CentalIdContainerAssetPath);
            
            if (centralIdContainer != null) 
            { 
                centralIdContainer.SearchForDuplicates();
            }
                
        }


       


    }
}

