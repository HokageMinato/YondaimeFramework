using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YondaimeFramework;


namespace YondaimeFramework.EditorHandles
{
    [CustomEditor(typeof(EditorIdContainer))]
    public class EditorIdContainerHashCallback : Editor
    {
        private static EditorCentalIdsDataSO dataSO;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
           
            if (HaveValuesBeenModified((EditorIdContainer)target))
            {
                RefreshIdHashValues();
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }

        }


        private static void RefreshIdHashValues()
        {
            if (dataSO == null)
                dataSO = AssetDatabase.LoadAssetAtPath<EditorCentalIdsDataSO>(ASSET_PATHS.CentalIdContainerAssetPath);
            
            dataSO.AssignId();
        }

        private static bool HaveValuesBeenModified(EditorIdContainer container) 
        {
            return EditorUtility.IsDirty(container);
        }



    }
}