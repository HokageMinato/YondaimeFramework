using System;
using UnityEngine;

namespace Tag
{
    public class ClosetView : BaseView,ITabHandlerSwitchCallback
    {
        #region PUBLIC_VARS
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private CharacterSelectionSubView _characterSelectionSubView;
        [SerializeField] private int tabIndex;
        #endregion

        #region UNITY_CALLBACKS

        #endregion

        #region PUBLIC_FUNCTIONS
        public override void InitView()
        {
            base.InitView();
            _characterSelectionSubView.Init();
        }

        public void OnTabSwitch(int switchedToIndex)
        {
            if (HasSwitchedToMyIndex(switchedToIndex))
            {
                ShowView();
            }
            else
            {
                HideView();
            }

         
            bool HasSwitchedToMyIndex(int switchedToId)
            {
                return switchedToId == tabIndex;
            }
        }




        #endregion

    }


    
}
