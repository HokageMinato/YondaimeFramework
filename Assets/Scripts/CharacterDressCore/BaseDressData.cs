using UnityEngine;



namespace Tag
{
    public class BaseDressData : ScriptableObject
    {
        #region PUBLIC_VARIABLES
        public Sprite uiDisplaySprite;
        public float price; //or IAP Id

        [Header("Set From DressesData SO")]
        public CharacterId characterId;

        #endregion
    }
}
