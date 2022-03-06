using YondaimeFramework;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace YondaimeFramework.EditorHandles
{
    [RequireComponent(typeof(SceneLibraryOld))]
    public class SceneLibraryHandle : MonoBehaviour
    {
        [SerializeField] SceneLibraryOld sceneLibrary;
        [SerializeField] SceneLibrary sceneLibraryNew;

        

        [ContextMenu("Scan")]
        public void ScanBehaviours() 
        {
            sceneLibrary = FindObjectOfType<SceneLibraryOld>();
            sceneLibrary.ScanBehaviours();
            sceneLibraryNew.GenerateBehaviourLibrary();
            SetSceneDirty();
        } 
        
       

        public void SetSceneDirty()
        {
            if (!Application.isPlaying)
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
         
    }
}
