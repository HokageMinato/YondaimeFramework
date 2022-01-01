
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tag
{
    [CustomPropertyDrawer(typeof(CharacterId))]

    public class CharacterIdDrawer : PropertyDrawer
    {
        
        #region UNITY_CALLBACKS
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            CharacterIdHelperSO characterDatasSo = LoadCharacterDataSOAsset();

            if (characterDatasSo != null)
            {
                List<string> characterIds =characterDatasSo.values;

                if (characterIds != null)
                {
                    float h = position.height / 2;
                    Rect rect2 = new Rect(position.x, position.y + h, position.width, h);

                    List<string> choices = characterIds;
                    int index = choices.IndexOf(property.FindPropertyRelative("id").stringValue);
                    index = EditorGUI.Popup(rect2, "CharacterId", index, choices.ToArray());

                    if (index != -1)
                        property.FindPropertyRelative("id").stringValue = choices[index];
                }
                else
                {
                    EditorGUI.LabelField(position, "EVENT ID NOT ASSIGNED");
                }
            }
            else
            {
                EditorGUI.LabelField(position, "EventIdHelper NOT FOUND");
            }

            EditorGUI.EndProperty();
        }

        private static CharacterIdHelperSO LoadCharacterDataSOAsset()
        {
            const string Path = "Assets/ScriptableObjects/HelperSO/CharacterIdHelperSo.asset";
            CharacterIdHelperSO charcterDatasSo = AssetDatabase.LoadAssetAtPath<CharacterIdHelperSO>(Path);
            return charcterDatasSo;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2;
        }

        #endregion
    }
}