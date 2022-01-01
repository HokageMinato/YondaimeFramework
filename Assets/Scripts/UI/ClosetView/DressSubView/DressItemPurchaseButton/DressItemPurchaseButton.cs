using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tag
{
    public class DressItemPurchaseButton : CustomBehaviour
    {

        #region PRIVATE_VARS
        [SerializeField] private Text _priceText;
        private BaseDressData myData;
        private List<Action<BaseDressData>> _onPurchaseSuccessCallback = new List<Action<BaseDressData>>();
        #endregion

        #region UNITY_CALLBACKS

        #endregion

        #region PUBLIC_FUNCTIONS
        public void Init(BaseDressData dressData) {
            _priceText.text = dressData.price.ToString();
            myData = dressData;
        }

        public void RegisterOnPurchaseSuccessCallbacks(Action<BaseDressData> onPurhaseSuccess) {
            if(!_onPurchaseSuccessCallback.Contains(onPurhaseSuccess))
                _onPurchaseSuccessCallback.Add(onPurhaseSuccess);
        }
        #endregion

        #region PRIVATE_FUNCTIONS
        private void InvokeOnPurchase() {
            for (int i = 0; i < _onPurchaseSuccessCallback.Count; i++)
            {
                _onPurchaseSuccessCallback[i](myData);
            }
        }
        #endregion

        #region CO-ROUTINES

        #endregion

        #region EVENT_HANDLERS

        #endregion

        #region UI_CALLBACKS       
        public void OnPurchaseButton() {
            InvokeOnPurchase();
        }
        #endregion
    }

    public interface IDressItemPurchaseCallbackDependency
    {
        public void OnDressPurchasedCallback(BaseDressData dressData);
    }

}
