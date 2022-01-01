using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YondaimeFramework
{

  public abstract class BaseCharacterDress : CustomBehaviour
    {
        #region PUBLIC_VARIABLES
        public BaseDressData dressData;
        #endregion


        #region PUBLIC_METHODS
        public abstract void Apply();

        public abstract void Preview(bool toggleValue);
        #endregion
    }
}