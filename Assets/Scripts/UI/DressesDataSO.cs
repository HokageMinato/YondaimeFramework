using UnityEditor;
using UnityEngine;

namespace Tag
{
    [CreateAssetMenu(fileName = "DressesDataSo", menuName = "ClosetSystem/ClosetViewDresses", order = 1)]
    public class DressesDataSO : ScriptableObject
    {
        #region PUBLIC_VARS
        public CharacterId CharacterId
        {
            get
            {
                return _characterId;
            }
        }

        public int TotalDressCount
        {
            get {
                return _dressData.Length;
            }
        }
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private CharacterId _characterId;
        [SerializeField] private BaseDressData[] _dressData;
        #endregion

        #region UNITY_CALLBACKS

        #endregion

        #region PUBLIC_FUNCTIONS
        public BaseDressData GetDressDataAtIndex(int index) {
            return _dressData[index];
        }
        #endregion

        #region PRIVATE_FUNCTIONS

        #endregion

        #region CO-ROUTINES

        #endregion

        #region EVENT_HANDLERS

        #endregion

        #region UI_CALLBACKS       

        #endregion

        #if UNITY_EDITOR
        [ContextMenu("Assign ID")]
        public void AssignId() {
            foreach (var item in _dressData)
            {
                item.characterId = _characterId;
                EditorUtility.SetDirty(item);
            }
        }
        #endif
    }
}
