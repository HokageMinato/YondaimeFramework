using YondaimeFramework;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace YondaimeFramework.EditorHandles
{
    [RequireComponent(typeof(SceneLibrary))]
    public class SceneLibraryHandle : MonoBehaviour
    {
        [SerializeField] SceneLibrary sceneLibrary;

        

        [ContextMenu("Scan")]
        public void ScanBehaviours()
        {
            FindSceneLibrary();
            SetSceneLibrary();
            CheckSceneLibraryForPrefab();
            SetSceneDirty();
        }

       

        private void FindSceneLibrary() 
        { 
            sceneLibrary = FindObjectOfType<SceneLibrary>();
        }

        private void SetSceneLibrary()
        {
            CustomBehaviour[] bhvs =FindObjectsOfType<CustomBehaviour>();
            sceneLibrary.SetBehaviours(bhvs);
            foreach (var item in bhvs)
            {
                item.SetLibrary(sceneLibrary);
                CheckForPrefabModifications(item);
            }
        }

        public void SetSceneDirty()
        {
            if (!Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        private void CheckForPrefabModifications(CustomBehaviour item)
        {
            if (IsPrefab(item)) 
            {
                ApplyModificationsAsOverrideToPrefab(item);
            }
        }

        private bool IsPrefab(MonoBehaviour behaviour) 
        { 
            return PrefabUtility.IsPartOfAnyPrefab(behaviour);
        }

        private void ApplyModificationsAsOverrideToPrefab(CustomBehaviour customBehaviour) 
        { 
            PrefabUtility.RecordPrefabInstancePropertyModifications(customBehaviour);
        } 
        
        private void CheckSceneLibraryForPrefab() 
        {
            if(IsPrefab(sceneLibrary))
             PrefabUtility.RecordPrefabInstancePropertyModifications(sceneLibrary);
        }

    }
}
