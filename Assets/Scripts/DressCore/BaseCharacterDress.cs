using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tag
{

  public abstract class BaseCharacterDress : CustomBehaviour
    {
        #region PUBLIC_VARIABLES
        public BaseDressData dressData;

        public abstract void Apply();

        public abstract void Preview(bool toggleValue);
        #endregion
    }
}