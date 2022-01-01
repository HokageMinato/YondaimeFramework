using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tag
{
    [System.Serializable]
    public struct CharacterId 
    {
        #region PUBLIC_VARIABLES
        public string id;
        #endregion

        #region PUBLIC_METHODS
        public bool Equals(CharacterId otherId) {
            return id == otherId.id;
        }
        #endregion
    }
}
