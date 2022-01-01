using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace YondaimeFramework
{
    public class DressItemView : CustomBehaviour
    {
        #region PUBLIC_VARS
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private Image _dressImage;
        [SerializeField] private DressItemPurchaseButton _purchaseButton;

        public DressItemPurchaseButton PurchaseButton{
            get{
                return _purchaseButton;
            }
        }
        private List<Action<BaseDressData>> _onItemSelectedActions= new List<Action<BaseDressData>>();
        private BaseDressData _dressData;
        #endregion

        
        #region PUBLIC_FUNCTIONS
        public void Init(BaseDressData dressData) {
            
            _dressData = dressData;
            InitializePurchaseButton();
            UpdateView();
            ResolveMyDependencies();
        }

        private void ResolveMyDependencies() {
            
            _onItemSelectedActions.Clear();
            List<IDressItemViewClickCallback> callbacks = GetComponentsFromLibrary<IDressItemViewClickCallback>();
            for (int i = 0; i < callbacks.Count; i++)
            {
                RegisterOnDressSelected(callbacks[i].OnDressSelectedCallback);
            }
        }

        public void RegisterOnDressSelected(Action<BaseDressData> itemSelectedAction) {
           _onItemSelectedActions.Add(itemSelectedAction);
        }

      
        #endregion

        #region PRIVATE_FUNCTIONS
        private void UpdateView() {
            _dressImage.sprite = _dressData.uiDisplaySprite;
        }

        private void OnDressItemSelectedInvoker() {
            for (int i = 0; i < _onItemSelectedActions.Count; i++)
            {
                _onItemSelectedActions[i](_dressData);
            }
        }

        private void InitializePurchaseButton()
        {
            _purchaseButton.Init(_dressData);
        }
        #endregion

        #region CO-ROUTINES

        #endregion

        #region EVENT_HANDLERS

        #endregion

        #region UI_CALLBACKS       
        public void OnItemViewClick() {
            OnDressItemSelectedInvoker();
        }
        #endregion
    }

    public interface IDressItemViewClickCallback
    {
        public void OnDressSelectedCallback(BaseDressData dressData);

    }



}
