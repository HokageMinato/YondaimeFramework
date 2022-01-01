using System.Collections.Generic;
using UnityEngine;

namespace Tag
{
    [CreateAssetMenu(fileName = "CharacterTypeContainerSO", menuName = "ClosetSystem/CharacterTypeSo", order = 1)]
    public class CustomerTypeContainerSO : ScriptableObject
    {
        #region PUBLIC_VARS

        #endregion

        #region PRIVATE_VARS
        [SerializeField] private float _probability;
        [SerializeField] private CharacterId[] _customers;
        
        private HashSet<CharacterId> idLookup = new HashSet<CharacterId>();
        #endregion

        #region UNITY_CALLBACKS

        #endregion

        #region PUBLIC_FUNCTIONS
        public bool HasId(CharacterId otherId) {
           
            for (int i = 0; i < _customers.Length; i++)
            {
                if (_customers[i].Equals(otherId))
                    return true;
            }

            return false;
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
