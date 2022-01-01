using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YondaimeFramework
{
    public class CharacterItemView : CustomBehaviour
    {
        #region PUBLIC_VARS
        public DressesDataSO CharacterId
        {
            get {
                return _dressData;
            }
        }
        #endregion

        #region PRIVATE_VARS
        private List<Action<DressesDataSO>> onClickActions = new List<Action<DressesDataSO>>();

        public void RegisterOnCharacterSelected(Action<DressesDataSO> onClick)
        {
            if(!onClickActions.Contains(onClick))
                onClickActions.Add(onClick);
        }

        public void OnClick()
        {
            for (int i = 0; i < onClickActions.Count; i++)
            {
                onClickActions[i](_dressData);
            }
        }

        private DressesDataSO _dressData;
        #endregion

        #region UNITY_CALLBACKS

        #endregion

        #region PUBLIC_FUNCTIONS
        public void Init(DressesDataSO dressData)
        {
            _dressData = dressData;
            gameObject.SetActive(true);
            ResolveMyDependencies();
        }

        private void ResolveMyDependencies() {
            List<ICharacterItemViewCallback> callbacks = GetComponentsFromLibrary<ICharacterItemViewCallback>();
            for (int i = 0; i < callbacks.Count; i++)
            {
                RegisterOnCharacterSelected(callbacks[i].OnCharacterSelected);
            }
        }

        public void HightlightView() {
            ToggleHightlight(true);
        }

        public void DeHighlightView() {
            ToggleHightlight(false);
        }
        
        
        #endregion

        #region PRIVATE_FUNCTIONS
        private void ToggleHightlight(bool toggleValue) {
            if (toggleValue)
                GetComponent<Image>().color = Color.green;
            else
                GetComponent<Image>().color = Color.white;
        }
       
        #endregion

        #region CO-ROUTINES

        #endregion

        #region EVENT_HANDLERS

        #endregion

        #region UI_CALLBACKS       
      
        #endregion
    }

    public interface ICharacterItemViewCallback
    {
        public void OnCharacterSelected(DressesDataSO dressData);
    }


}
