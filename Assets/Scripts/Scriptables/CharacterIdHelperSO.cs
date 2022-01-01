using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    [CreateAssetMenu(fileName = "CharacterIdHelperSo", menuName = "ClosetSystem/CharacterIdHelper", order = 1)]
    public class CharacterIdHelperSO : ScriptableObject
    {
        #region PUBLIC_VARS
        public List<string> values;
        #endregion

     }
}
