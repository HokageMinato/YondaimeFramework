using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tag
{
    public class BaseView : CustomBehaviour
    {
        #region PUBLIC_VARS

        #endregion

        #region PRIVATE_VARS

        #endregion

        #region UNITY_CALLBACKS
        #endregion

        #region PUBLIC_FUNCTIONS
        public virtual void InitView() 
        {
        }


        public virtual void SetView() { }
        public virtual void ShowView() {
            SetView();
            gameObject.SetActive(true); 
        }
        public virtual void HideView() { 
        
            gameObject.SetActive(false); 
        
        }
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
