using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YondaimeFramework
{
    public class DataNamePreviewView : CustomBehaviour, ICharacterItemViewCallback
    {
        #region PUBLIC_VARS
        public Text txt;
        #endregion

        #region PRIVATE_VARS

        #endregion

        #region UNITY_CALLBACKS

        #endregion

        #region PUBLIC_FUNCTIONS
        public void OnCharacterSelected(DressesDataSO dressData)
        {
            txt.text = dressData.CharacterId.id;
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
