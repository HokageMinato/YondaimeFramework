using UnityEditor;
using YondaimeFramework;
using UnityEngine;

namespace YondaimeFramework.EditorHandles
{

    [CustomEditor(typeof(RuntimeIdContainer))]
    public class RuntimeIdInspectorDisplay : Editor
    {

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();

            RuntimeIdContainer runtimeIdContainer = (RuntimeIdContainer)target;
            ComponentId[] ids = runtimeIdContainer.GetIds();
               DrawTitle();
                DrawIds(ids);
        }

        private static void DrawTitle()
        {
            EditorGUILayout.LabelField($"Available ids:");
        }
        
        

        private static void DrawIds(ComponentId[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                EditorGUILayout.LabelField($"Id : {ids[i].stringId}", $"Hash : {ids[i].objBt}");
            }
        }
    }
}
