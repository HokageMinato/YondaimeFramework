using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


namespace YondaimeFramework
{
    [CustomPropertyDrawer(typeof(ComponentId))]
    public class ComponentIdDrawer : PropertyDrawer
    {
        #region UNITY_CALLBACKS
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            string[] systemIds = GetId();
            
            if (systemIds != null)
            {
                float h = position.height / 2;
                Rect rect2 = new Rect(position.x, position.y + h, position.width, h);

                string[] choices = systemIds;
                int index = ArrayUtility.IndexOf(choices, property.FindPropertyRelative("id").stringValue);
                index = EditorGUI.Popup(rect2, "CompId", index, choices);

                if (index != -1)
                    property.FindPropertyRelative("id").stringValue = choices[index];
            }


            EditorGUI.EndProperty();
        }

        private static string[] GetId()
        {
            const string Path = "Assets/YondaimeFramework/Scriptables/ComponentIds/ComponentIdContainer.asset";
            ComponentIdSources componentIdSources = AssetDatabase.LoadAssetAtPath<ComponentIdSources>(Path);
            return componentIdSources.GetIds();
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2;
        }

        #endregion
    }
}