using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace YondaimeFramework
{
    public class DressDisplaySubView : CustomBehaviour, ICharacterItemViewCallback
    {
        #region PUBLIC_VARS
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private List<DressItemView> _dressItemViews;
        private DressesDataSO _activeDressDatas;
        #endregion

        #region UNITY_CALLBACKS

        #endregion

        #region PUBLIC_FUNCTIONS

        public void OnCharacterSelected(DressesDataSO dressData)
        {
            _activeDressDatas = dressData;
            GenerateSufficientDressItemViews();
            InitItemViews();
        }


        #endregion

        #region PRIVATE_FUNCTIONS

        private void GenerateSufficientDressItemViews()
        {

            int totalDressesToDisplay = _activeDressDatas.TotalDressCount;

            if (!AreEnoughViewsInstantiated())
            {
                ModifyViewCountToRequiredAmount();
                RefreshHierarchy();
            }


            bool AreEnoughViewsInstantiated()
            {
                return (_dressItemViews.Count == totalDressesToDisplay);
            }
            void ModifyViewCountToRequiredAmount()
            {

                if (_dressItemViews.Count < totalDressesToDisplay)
                {
                    GenerateExtraViews();
                    
                }
                if (_dressItemViews.Count > totalDressesToDisplay)
                {
                    DestoryExtraViews();
                    
                }

            }
            void GenerateExtraViews()
            {
                DressItemView activePrefab = _dressItemViews[0];
                int extraDressItemViewRequired = totalDressesToDisplay - _dressItemViews.Count;

                for (int i = 0; i < extraDressItemViewRequired; i++)
                {
                    _dressItemViews.Add(Instantiate(activePrefab, transform));
                    _dressItemViews[i].SetLibrary(MyLibrary);
                }

            }
            void DestoryExtraViews()
            {

                for (int i = _dressItemViews.Count - 1; i >= totalDressesToDisplay; i--)
                {
                    DressItemView itemView = _dressItemViews[i];
                    _dressItemViews.RemoveAt(i);
                    itemView.DestorySelf();
                }

                
            }
        }
        

        private void InitItemViews()
        {
            for (int i = 0; i < _activeDressDatas.TotalDressCount; i++)
            {
                BaseDressData dressData = _activeDressDatas.GetDressDataAtIndex(i);
                DressItemView itemView = _dressItemViews[i];
                itemView.Init(dressData);
            }

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
