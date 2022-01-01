using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tag
{
    [CreateAssetMenu(fileName = "CharacterDataSo", menuName = "ClosetSystem/CharacterData", order = 1)]
    public class CustomerCharactersDataSO : ScriptableObject
    {
        #region PUBLIC_VARS
        public DressesDataSO[] characterDressDatas;

        public int CharacterCount
        {
            get {
                return characterDressDatas.Length;
            }
        }
        #endregion

        #region PRIVATE_VARS

        #endregion

        #region UNITY_CALLBACKS

        #endregion

        #region PUBLIC_FUNCTIONS
        public DressesDataSO GetIdStructOfIndex(int idx) {
            return characterDressDatas[idx];
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
    }
}
