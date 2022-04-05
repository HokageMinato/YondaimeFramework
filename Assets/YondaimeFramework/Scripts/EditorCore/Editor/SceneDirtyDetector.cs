using UnityEditor;
using UnityEditor.SceneManagement;

namespace YondaimeFramework.EditorHandles{

    [InitializeOnLoad]
    public class SceneDirtyDetector : CustomBehaviour
    {
        static SceneDirtyDetector()
        {
            EditorApplication.update += () =>
            {
                if (EditorSceneManager.GetActiveScene().isDirty)
                {
                    InvokeBehavioursScan();
                }
            };
        }


        private static void InvokeBehavioursScan()
        {
            LibraryHandle handle = FindHandleFromScene();

            if (handle == null) 
                MissingLibraryException();
            
            handle.ScanBehaviours();
        }


        static LibraryHandle FindHandleFromScene() 
        {
            return FindObjectOfType<LibraryHandle>();
        }


        static void MissingLibraryException()
        {
            throw new System.Exception("Please create a library or if already present please attach a library handle or framework functionalities wont work properly");
        }

    }

}