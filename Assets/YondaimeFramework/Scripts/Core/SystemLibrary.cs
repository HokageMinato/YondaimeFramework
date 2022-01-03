using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YondaimeFramework
{
    public class SystemLibrary : BehaviourLibrary
    {
        #region PUBLIC_VARS
        [SerializeField] private SystemId systemId;
        #endregion

      
        #region PUBLIC_FUNCTIONS

        public void SetSystemLibrary()
        {
            CustomBehaviour[] behaviours = GetComponentsInChildren<CustomBehaviour>();
            for (int i = 0; i < behaviours.Length; i++)
            {
                behaviours[i].SetSystemLibrary(this);
            }
        }
        #endregion

        #region PRIVATE_FUNCTIONS

        #endregion

        #region CO-ROUTINES

        #endregion

        #region EVENT_HANDLERS

        #endregion

        #region UI_CALLBACKS       

        [ContextMenu("Scan")]
        public override void ScanBehaviours() {
            base.ScanBehaviours();
            SetSystemLibrary();
        }
        #endregion
    }
}
