using UnityEngine;
using UnityEditor;
using YondaimeFramework;

namespace YondaimeFramework
{
    [CustomEditor(typeof(CustomBehaviour),editorForChildClasses:true)]
    [CanEditMultipleObjects]
    public class ComponentIdDrawer : Editor
    {
        private const string SET_OBJECT_ID = "Set Object Id";
        private const string CHANGE_OBJECT_ID = "Change Object Id";


        public override void OnInspectorGUI()
        {
            CentalIdsDataSO idSources=null;
            CustomBehaviour targetBehaviour = target as CustomBehaviour; 
            string targetObjectId = targetBehaviour.ObjectId;

            GUILayout.BeginHorizontal();
            if (IsIdSourceNull())
            {
                LoadIdSource();
            }

            if (IsIdSourceNull())
            {
                ShowWarningLabel();
                   return;
            }

            DrawIdLabel();

            if (DrawObjectIdButton())
                DrawMenu();

            GUILayout.EndHorizontal();


            base.OnInspectorGUI();

            #region LOCAL_FUNCTIONS

            void DrawMenu()
            {
                GenericMenu menu = new GenericMenu();
                FillMenu(menu);
                menu.ShowAsContext();
            }

            void OnIdSelected(object id)
            {
                targetBehaviour.SetObjectId((string)id);
                EditorUtility.SetDirty(targetBehaviour.gameObject);
            }

            void RemoveId(object id) 
            {
                targetBehaviour.SetObjectId(string.Empty);
            }

            void FillMenu(GenericMenu targetMenu)
            {
                SystemIdsData[] systemIdData = idSources.SystemIdsData;
                
                AddNoneOption(targetMenu);

                for (int i = 0; i < systemIdData.Length; i++)
                {
                    AddSystemIdSubMenu(targetMenu, systemIdData[i]);
                }
            }

            void AddNoneOption(GenericMenu targetMenu)
            {
                string choiceId = CentalIdsDataSO.None;
                bool isPresentItemSelected = IsEmpty(targetObjectId);
                targetMenu.AddItem(new GUIContent($"{choiceId}"), isPresentItemSelected, RemoveId, choiceId);
            }

            void AddSystemIdSubMenu(GenericMenu targetMenu, SystemIdsData idData)
            {
                string systemId = idData.SystemId;
                string[] componentIds = idData.GetIds();

                for (int i = 0; i < componentIds.Length; i++)
                {
                    string choiceId = componentIds[i];
                    bool isPresentItemSelected = !IsEmpty(targetObjectId) && targetObjectId.Equals(choiceId);
                    targetMenu.AddItem(new GUIContent($"{systemId}/{choiceId}"), isPresentItemSelected, OnIdSelected, choiceId);
                }
            }

            bool DrawObjectIdButton()
            {
                string buttonString = IsEmpty(targetObjectId) ? SET_OBJECT_ID : CHANGE_OBJECT_ID;
                return GUILayout.Button(buttonString);
            }

            void DrawIdLabel()
            {
                if (!IsEmpty(targetObjectId))
                    EditorGUILayout.LabelField("Object Id:",targetObjectId);
            }

            void ShowWarningLabel() 
            {
                EditorGUILayout.LabelField("Id Source Empty Or Not Found");
            }

            void LoadIdSource()
            {
                idSources = AssetDatabase.LoadAssetAtPath<CentalIdsDataSO>("Assets/YondaimeFramework/Scriptables/ComponentIds/ComponentIdContainer.asset");
            }

            bool IsIdSourceNull()
            {
                return idSources == null;
            }

            bool IsEmpty(string id)
            {
                return string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id);
            }

            #endregion
        }
    }
}