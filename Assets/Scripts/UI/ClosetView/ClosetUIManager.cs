using UnityEngine;

namespace YondaimeFramework
{
    public class ClosetUIManager : CustomBehaviour
    {
        #region PUBLIC_VARS
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private BaseView[] views;
        [SerializeField] private TabHandler tabHandler;
        #endregion

        #region UNITY_CALLBACKS
        private void Awake()
        {
            InitViews();
            InitTabHandler();
         
        }

        private void InitTabHandler() {
            tabHandler.Init();
        }

        private void InitViews()
        {
            ParentLibrary.InitializeLibrary();

            for (int i = 0; i < views.Length; i++)
            {
                views[i].InitView();
            }
            
        }
        #endregion

        #region PUBLIC_FUNCTIONS

        #endregion

        #region PRIVATE_FUNCTIONS
        #endregion

        #region CO-ROUTINES

        #endregion

        #region EVENT_HANDLERS

        #endregion

        #region UI_CALLBACKS      
        
        #endregion
    }
}
