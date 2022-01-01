using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tag
{
    public class OutfitPackView : BaseView, ITabHandlerSwitchCallback
    {
        #region PUBLIC_VARS
        [SerializeField] private int myTabIndex;
        #endregion

        #region PRIVATE_VARS

        #endregion

        #region UNITY_CALLBACKS

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

        public void OnTabSwitch(int switchedToId)
        {
            if (HasSwitchedToMyIndex(switchedToId))
            {
                ShowView();
            }
            else
            {
                HideView();
            }


            bool HasSwitchedToMyIndex(int switchedToId)
            {
                return switchedToId ==myTabIndex;
            }
        }
    }
}
