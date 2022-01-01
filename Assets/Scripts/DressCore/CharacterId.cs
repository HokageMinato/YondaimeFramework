using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tag
{
    [System.Serializable]
    public struct CharacterId 
    {
        public string id;

        public bool Equals(CharacterId otherId) {
            return id == otherId.id;
        }
    }
}
