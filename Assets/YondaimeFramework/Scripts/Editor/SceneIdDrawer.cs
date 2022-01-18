using UnityEngine;
using UnityEditor;
using System.Reflection;


namespace YondaimeFramework
{
    [CustomPropertyDrawer(typeof(SceneId))]
    public class SceneIdDrawer : PropertyDrawer
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
                index = EditorGUI.Popup(rect2, "SceneId", index, choices);

                if (index != -1)
                    property.FindPropertyRelative("id").stringValue = choices[index];
            }


            EditorGUI.EndProperty();
        }

        private static string[] GetId()
        {
            FieldInfo[] infos = typeof(FrameworkConstants.IdConstants).GetFields();
            string[] ids = new string[infos.Length];

            for (int i = 0; i < infos.Length; i++)
            {
                ids[i] = infos[i].Name;
            }

            return ids;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2;
        }

        #endregion
    }
}
