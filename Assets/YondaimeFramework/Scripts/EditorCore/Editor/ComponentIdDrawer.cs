using UnityEngine;
using UnityEditor;
using YondaimeFramework;
using System.Collections.Generic;

namespace YondaimeFramework.EditorHandles
{
    [CustomPropertyDrawer(typeof(ComponentId))]
    public class ComponentIdDrawer : PropertyDrawer
    {
        EditorCentalIdsDataSO idSources = null;


        //Make sure the values of these are same as the ComponentId class fields
        //or editor will not be able to serialize them properly
        private const string StringIdPropertyName = "stringId";
        private const string IntIdValName = "objBt";
        private const string NoneStr = "None";

        //Make sure all path based operations are done via this, since this will decide the folder hierarcy
        
        

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
                idSources = AssetDatabase.LoadAssetAtPath<EditorCentalIdsDataSO>(ASSET_PATHS.CentalIdContainerAssetPath);
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
                string choiceId = NoneStr;
                contentList.Add(new GUIContent(choiceId));

                actualValuesList.Add(new ComponentId
                {
                    stringId = "None",
                    objBt = ComponentId.None
                });
            }

            void AddSystemIdSubMenu(List<GUIContent> contentList, List<ComponentId> choiceList, SystemIdsData idData)
            {
                string systemId = idData.SystemId;
                ComponentIdSRC[] componentIds = idData.GetIdSRCs();

                for (int i = 0; i < componentIds.Length; i++)
                {
                    string choiceId = componentIds[i].stringIdVal;
                    contentList.Add(new GUIContent($"{systemId}/{choiceId}"));
                    choiceList.Add(componentIds[i].ParseToRuntimeId());
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
                return property.FindPropertyRelative(StringIdPropertyName).stringValue;
            }
            
            void SetPresentStringIdValueToTargetProperty(string value,int idVal) 
            {
                property.FindPropertyRelative(StringIdPropertyName).stringValue=value;
                property.FindPropertyRelative(IntIdValName).intValue = idVal;
            }
            #endregion

        }



    }
}
