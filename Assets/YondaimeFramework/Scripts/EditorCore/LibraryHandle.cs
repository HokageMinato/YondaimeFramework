using YondaimeFramework;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace YondaimeFramework.EditorHandles
{
    [RequireComponent(typeof(ILibrary))]
    public class LibraryHandle : MonoBehaviour
    {
        ILibrary sceneLibrary;
        

        [ContextMenu("Scan")]
        public void ScanBehaviours()
        {
            FindSceneLibrary();
            ScanCustomBehaviours();
            CheckSceneLibraryForPrefab();
            SetSceneDirty();
        }

        private void ScanCustomBehaviours()
        {
            sceneLibrary.SetBehaviours(FindObjectsOfType<CustomBehaviour>());
        }

        private void FindSceneLibrary() 
        { 
            MonoBehaviour[] behv = FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour b in behv) 
            {
                if (b is ILibrary)
                {
                    sceneLibrary = (ILibrary)b;
                    return;
                }
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
           // if(IsPrefab(sceneLibrary))
            // PrefabUtility.RecordPrefabInstancePropertyModifications(sceneLibrary);
        }

    }
}
