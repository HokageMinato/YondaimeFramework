using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tag
{
    public class CharacterSelectionSubView : CustomBehaviour, ICharacterItemViewCallback
    {
        #region PUBLIC_VARS
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private CustomerCharactersDataSO customerCharacters;
        [SerializeField] private CharacterItemView[] characterItemViews;
        //[SerializeField] private ItemFactory _itemFactory;

        private DressesDataSO _selectedDressData;
        private Dictionary<DressesDataSO, int> itemViewMap = new Dictionary<DressesDataSO, int>();

        #endregion


        #region PUBLIC_FUNCTIONS
        public void Init()
        {
            GenerateCharacterItemViews();
            SetView();
        }


        public void SetView()
        {
            UpdateActiveView(customerCharacters.characterDressDatas[0]);
            HighlightActiveView();
        }

        public void OnCharacterSelected(DressesDataSO dressData)
        {
            DehighlightAllViews();
            UpdateActiveView(dressData);
            HighlightActiveView();
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void GenerateCharacterItemViews()
        {
            int totalCharacters = customerCharacters.CharacterCount;
            //characterItemViews = _itemFactory.GenerateItems<CharacterItemView>(totalCharacters, transform);


            for (int i = 0; i < totalCharacters; i++)
            {
                DressesDataSO dressDataForItemView = customerCharacters.GetIdStructOfIndex(i);
                characterItemViews[i].Init(dressDataForItemView);
                itemViewMap.Add(dressDataForItemView, i);
            }

           
        }

        private void UpdateActiveView(DressesDataSO newDressData)
        {
            _selectedDressData = newDressData;
        }

        private void DehighlightAllViews()
        {
            for (int i = 0; i < characterItemViews.Length; i++)
            {
                characterItemViews[i].DeHighlightView();
            }
        }

        private void HighlightActiveView()
        {
            int itemIndex = itemViewMap[_selectedDressData];
            characterItemViews[itemIndex].HightlightView();
        }

        

        #endregion

        #region CO-ROUTINES

        #endregion

        #region EVENT_HANDLERS

        #endregion

        #region UI_CALLBACKS       

        #endregion
    }


}
