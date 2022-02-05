using UnityEngine;
using UnityEditor;
using YondaimeFramework;
using System.Collections.Generic;

namespace YondaimeFramework
{
    [CustomPropertyDrawer(typeof(ComponentId))]
    public class ComponentIdDrawer : PropertyDrawer
    {
        CentalIdsDataSO idSources = null;
        

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.BeginProperty(position, label, property);
            
            if (IsIdSourceNull())
            {
                LoadIdSource();
            }

            if (IsIdSourceNull()) 
            {
                EditorGUI.LabelField(position, new GUIContent("No Id Source Present,Create or Check Path"));
                return;
            }

            GUIContent labelToDisplay = new GUIContent(property.name);
            GUIContent[] menu;
            ComponentId[] idChoices;
             
            FillChoiceMenu();

            string presentStringIdValue = GetPresentStringIdValueFromTargetProperty();
            
            int oldIndex = GetIndexBasedOf(presentStringIdValue); 

            int newIndex = EditorGUI.Popup(position,labelToDisplay, oldIndex,menu);

            if (newIndex != -1)
                SetPresentStringIdValueToTargetProperty(idChoices[newIndex].stringId, idChoices[newIndex].objBt);
            

            EditorGUI.EndProperty();



            #region FUNCTION_DECLARATIONS

            bool IsIdSourceNull()
            {
                return idSources == null;
            }
           
            void LoadIdSource()
            {
                idSources = AssetDatabase.LoadAssetAtPath<CentalIdsDataSO>("Assets/YondaimeFramework/Scriptables/ComponentIds/ComponentIdContainer.asset");
            }

            void FillChoiceMenu()
            {
                SystemIdsData[] systemIdData = idSources.SystemIdsData;
                List<GUIContent> contentList = new List<GUIContent>();
                List<ComponentId> choices = new List<ComponentId>();

                AddNoneOption(contentList,choices);

                for (int i = 0; i < systemIdData.Length; i++)
                {
                    AddSystemIdSubMenu(contentList, choices,systemIdData[i]);
                }

                menu = contentList.ToArray();
                idChoices = choices.ToArray();
            }

            void AddNoneOption(List<GUIContent> contentList,List<ComponentId> actualValuesList)
            {
                string choiceId = ComponentId.NoneStr;
                contentList.Add(new GUIContent(choiceId));
                actualValuesList.Add(new ComponentId
                {
                    stringId = "None",
                    objBt = -1
                });
            }

            void AddSystemIdSubMenu(List<GUIContent> contentList, List<ComponentId> choiceList, SystemIdsData idData)
            {
                string systemId = idData.SystemId;
                ComponentIdSRC[] componentIds = idData.GetIds();

                for (int i = 0; i < componentIds.Length; i++)
                {
                    string choiceId = componentIds[i].stringIdVal;
                    contentList.Add(new GUIContent($"{systemId}/{choiceId}"));
                    choiceList.Add(new ComponentId(componentIds[i]));
                }
            }
            
            int GetIndexBasedOf(string presentValue) 
            {
                
                for (int i = 0; i < idChoices.Length; i++)
                {
                    if (idChoices[i].stringId == presentValue)
                    {
                        return i;
                    }
                }

                return ComponentId.None;
            }

            string GetPresentStringIdValueFromTargetProperty() 
            {
                return property.FindPropertyRelative(ComponentId.StringIdPropertyName).stringValue;
            }
            
            void SetPresentStringIdValueToTargetProperty(string value,int idVal) 
            {
                property.FindPropertyRelative(ComponentId.StringIdPropertyName).stringValue=value;
                property.FindPropertyRelative(ComponentId.IntIdValName).intValue = idVal;
            }
            #endregion

        }



    }
}