using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace YondaimeFramework.EditorHandles
{
    [CustomPropertyDrawer(typeof(SceneId))]
    public class SceneIdDrawer : PropertyDrawer
    {
        #region UNITY_CALLBACKS
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string[] systemIds = GetId();
            
            if (systemIds != null)
            {
                Rect fieldDrawPosition = new Rect(position);

                string[] choices = systemIds;
                int index = ArrayUtility.IndexOf(choices, property.FindPropertyRelative("id").stringValue);
                index = EditorGUI.Popup(fieldDrawPosition, "SceneId", index, choices);

                if (index != -1)
                    property.FindPropertyRelative("id").stringValue = choices[index];
            }
        }

        private static string[] GetId()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            string[] ids = new string[sceneCount];

            for (int i = 0; i < sceneCount; i++)
            {
                ids[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }

            return ids;
        }


        #endregion
    }
}
