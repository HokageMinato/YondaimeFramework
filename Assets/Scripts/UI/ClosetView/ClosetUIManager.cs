using UnityEngine;

namespace Tag
{
    public class ClosetUIManager : CustomBehaviour
    {
        #region PUBLIC_VARS
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private BaseView[] views;
        #endregion

        #region UNITY_CALLBACKS
        private void Awake()
        {
            InitViews();
        }

        private void InitViews()
        {
            for (int i = 0; i < views.Length; i++)
            {
                views[i].InitView();
            }
            ParentLibrary.InitializeLookUp();
        }
        #endregion

       
    }
}
