using Spine.Unity;
using UnityEngine;



namespace Tag
{

    [RequireComponent(typeof(SkeletonAnimation))]
    public class SpineSkinDress : BaseCharacterDress
    {
        #region PUBLIC_VARIABLES
        public SkeletonAnimation targetCharacter;
        public GameObject root;
        #endregion

        #region PUBLIC_METHODS
        public override void Apply()
        {
            SpineDressData spineDressData = (SpineDressData)dressData;
            //targetCharacter.skeleton.SetSkin(spineDressData.skinName);
        }

        public override void Preview(bool toggleValue)
        {
            root.gameObject.SetActive(toggleValue);
        }
        #endregion

    }
}