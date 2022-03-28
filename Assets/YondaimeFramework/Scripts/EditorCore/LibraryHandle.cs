using YondaimeFramework;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEditor;


namespace YondaimeFramework.EditorHandles
{
    [RequireComponent(typeof(ILibrary))]
    public class LibraryHandle : MonoBehaviour
    {
        ILibrary sceneLibrary;


        public void ScanBehaviours()
        {
            StartCoroutine(DelayedCallback(() => 
            {
                FindSceneLibrary();
                ScanCustomBehaviours();
                SetSceneDirty();
            }));
            
        }

        private IEnumerator DelayedCallback(Action invokee) 
        {
            yield return null;
            invokee();
        }
        
        private void ScanCustomBehaviours()
        {
            sceneLibrary.SetBehaviours(FindObjectsOfType<CustomBehaviour>(true));
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

        private bool IsPrefab(MonoBehaviour behaviour) 
        { 
            return PrefabUtility.IsPartOfAnyPrefab(behaviour);
        }

        private void ApplyModificationsAsOverrideToPrefab(CustomBehaviour customBehaviour) 
        { 
            PrefabUtility.RecordPrefabInstancePropertyModifications(customBehaviour);
        } 
        
    }
}
