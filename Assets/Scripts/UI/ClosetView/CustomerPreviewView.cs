using UnityEngine;

namespace Tag
{
    public class CustomerPreviewView : CustomBehaviour,IDressItemViewClickCallback,IDressItemPurchaseCallbackDependency
    {
        #region PUBLIC_VARS
        public CharacterPreview[] characterPreviews;
        
        #endregion

        #region PRIVATE_VARS
        #endregion

        #region UNITY_CALLBACKS

        #endregion

        #region PUBLIC_FUNCTIONS

        public void OnDressSelectedCallback(BaseDressData dressData)
        {
            OnDressDataUpdated(dressData);
        }

        public void OnDressPurchasedCallback(BaseDressData dressData)
        {
            OnDressDataUpdated(dressData);
        }

        #endregion

        #region PRIVATE_FUNCTIONS

        private void OnDressDataUpdated(BaseDressData dressData) {
            DisableAllPreviews();
            SetPreviewFor(dressData);
        }


        private void SetPreviewFor(BaseDressData dressData)
        {
            CharacterId characterId = dressData.characterId;
          
            for (int i = 0; i < characterPreviews.Length; i++)
            {
                 if (characterPreviews[i].characterId.Equals(characterId))
                {
                    characterPreviews[i].SetPreviewDressData(dressData);
                    characterPreviews[i].ShowPreview();
                    return;
                }
            }
        }

        private void DisableAllPreviews() {

            for (int i = 0; i < characterPreviews.Length; i++)
            {
                characterPreviews[i].HidePreview();
            }

        }

        #endregion

        #region CO-ROUTINES

        #endregion

        #region EVENT_HANDLERS

        #endregion

        #region UI_CALLBACKS       

        #endregion



        [System.Serializable]
        public class CharacterPreview {

            public CharacterId characterId;
            public BaseCharacterDress character;

            public void SetPreviewDressData(BaseDressData dressData) {
                character.dressData = dressData;
                character.Apply();
            }

            public void ShowPreview() {
                character.Preview(true);
            }

            public void HidePreview() {
                character.Preview(false);
            }
        }

    }
}
