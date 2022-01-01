using UnityEngine;


namespace YondaimeFramework
{
   [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class MeshDress : BaseCharacterDress
    {
        #region PUBLIC_VARIBLES
        public SkinnedMeshRenderer skinnedMeshedRenderer;
        public GameObject root;
        public CharacterId characterId;
        #endregion

        #region PUBLIC_METHODS
        public override void Apply()
        {
            skinnedMeshedRenderer.material = ((MeshDressData)dressData).material;
        }

        public override void Preview(bool toggleValue)
        {
            root.gameObject.SetActive(toggleValue);
        }
        #endregion
    }
}