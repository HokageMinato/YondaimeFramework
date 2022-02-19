#if UNITY_EDITOR
using YondaimeFramework;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace YondaimeFramework.EditorHandles
{
    [RequireComponent(typeof(SceneLibrary))]
    public class SceneLibraryHandle : MonoBehaviour
    {
        [SerializeField] SceneLibrary sceneLibrary;

        

        [ContextMenu("Scan")]
        public void ScanBehaviours() 
        {
            sceneLibrary= FindObjectOfType<SceneLibrary>();
            sceneLibrary.ScanBehaviours();
            SetSceneDirty();
        }

        public void SetSceneDirty()
        {
            if (!Application.isPlaying)
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
         
    }
}
#endif