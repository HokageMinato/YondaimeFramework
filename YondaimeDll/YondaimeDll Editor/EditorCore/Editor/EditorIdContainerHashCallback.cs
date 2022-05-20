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
        public const string ASSET_PATH = "Assets/YondaimeFramework/Scriptables/Editor Id Scriptables/_CentralIdsDataSO.asset";
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
                dataSO = AssetDatabase.LoadAssetAtPath<EditorCentalIdsDataSO>(ASSET_PATH);
            
            dataSO.AssignId();
        }

        private static bool HaveValuesBeenModified(EditorIdContainer container) 
        {
            return EditorUtility.IsDirty(container);
        }



    }
}