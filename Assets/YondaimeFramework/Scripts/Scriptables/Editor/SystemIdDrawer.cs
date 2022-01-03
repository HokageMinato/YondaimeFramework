using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace YondaimeFramework
{
    [CustomPropertyDrawer(typeof(SystemId))]
    public class SystemIdDrawer : PropertyDrawer
    {
        const string Path = "Assets/YondaimeFramework/ScriptableObjects/SystemIdContainer.asset";

        #region UNITY_CALLBACKS
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            DrawerContentContainerSO systemIdDatas = LoadCharacterDataSOAsset();

            if (systemIdDatas != null)
            {
                string[] systemIds = systemIdDatas.SystemIds;

                if (systemIds != null)
                {
                    float h = position.height / 2;
                    Rect rect2 = new Rect(position.x, position.y + h, position.width, h);

                    string[] choices = systemIds;
                    int index = ArrayUtility.IndexOf(choices, property.FindPropertyRelative("id").stringValue);
                    index = EditorGUI.Popup(rect2, "SystemId", index, choices);

                    if (index != -1)
                        property.FindPropertyRelative("id").stringValue = choices[index];
                }
            }
            else
            {
                EditorGUI.LabelField(position, $"System Id Scriptable Not Found at {Path}");
            }

            EditorGUI.EndProperty();
        }

        private static DrawerContentContainerSO LoadCharacterDataSOAsset()
        {
            
            DrawerContentContainerSO charcterDatasSo = AssetDatabase.LoadAssetAtPath<DrawerContentContainerSO>(Path);
            return charcterDatasSo;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2;
        }

        #endregion
    }
}
