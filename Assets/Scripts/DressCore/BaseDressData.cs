using UnityEngine;



namespace Tag
{
    public class BaseDressData : ScriptableObject
    {
        #region PUBLIC_VARIABLES
        public CharacterId characterId;
        public Sprite uiDisplaySprite;
        public float price; //or IAP Id
        #endregion
    }
}
