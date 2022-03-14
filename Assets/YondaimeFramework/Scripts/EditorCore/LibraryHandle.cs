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
            if (IsPooledLibrary())
                SetPooledLibraryParams();
            SetSceneDirty();
        }

        private void SetPooledLibraryParams()
        {
            PooledBehaviourLibrary lib = sceneLibrary as PooledBehaviourLibrary;
            PoolParameters pparams = lib.GetComponent<PoolParameters>();
            if (!pparams)
            {
               pparams = lib.gameObject.AddComponent<PoolParameters>();
            }
            
            lib.SetPoolParameters(pparams);

        }

        private bool IsPooledLibrary()
        {
            return sceneLibrary is PooledBehaviourLibrary;
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
