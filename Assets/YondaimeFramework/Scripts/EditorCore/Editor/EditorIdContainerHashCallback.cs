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

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
           
            if (HaveValuesBeenModified((EditorIdContainer)target))
            {
                RefreshIdHashValues();
                SaveNewValues();
            }

        }

        private static void SaveNewValues()
        {
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        private static void RefreshIdHashValues()
        {
            AssetDatabase.LoadAssetAtPath<EditorCentalIdsDataSO>(ASSET_PATH).AssignId();
        }

        private static bool HaveValuesBeenModified(EditorIdContainer container) 
        {
            return EditorUtility.IsDirty(container);
        }



    }
}