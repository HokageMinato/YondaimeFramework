using System;
using UnityEngine;

namespace Tag
{
    public class ClosetView : BaseView 
    {
        #region PUBLIC_VARS
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private CharacterSelectionSubView _characterSelectionSubView;
        #endregion

        #region UNITY_CALLBACKS

        #endregion

        #region PUBLIC_FUNCTIONS
        public override void InitView()
        {
            base.InitView();
            _characterSelectionSubView.Init();
        }

      


        #endregion

    }


    
}
