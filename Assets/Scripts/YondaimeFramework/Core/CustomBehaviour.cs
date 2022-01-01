using UnityEngine;

namespace Tag
{
    public abstract class CustomBehaviour : MonoBehaviour
    {
        [SerializeField] CustomBehaviourLibrary myLibrary;
        public CustomBehaviourLibrary ParentLibrary => myLibrary;

        public void SetLibrary(CustomBehaviourLibrary library) {
            myLibrary = library;
        }

        public CustomBehaviour[] GetComponentsFast<I>() 
        {
            return myLibrary.GetBehaviours<I>();
        }

        public CustomBehaviour GetComponentFast<I>() {
            return myLibrary.GetBehaviour<I>();
        }
        
        public CustomBehaviour[] GetComponentsInChildrenFast<I>() 
        {
            return myLibrary.GetBehavioursInChildren<I>();
        }

        public CustomBehaviour GetComponentInChildrenFast<I>() {
            return myLibrary.GetBehaviour<I>();
        }

        public void OnHierarchyRefresh() 
        {
            myLibrary.ScanTypes();
        }
    }
}
