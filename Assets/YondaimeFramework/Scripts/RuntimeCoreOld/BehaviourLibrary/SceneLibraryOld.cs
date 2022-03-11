using System;
using System.Collections.Generic;
using UnityEngine;

namespace YondaimeFramework
{
    public sealed class SceneLibraryOld : MonoBehaviour
    {
        #region PRIVATE_VARS
        public CustomBehaviour[] bhv;
        public StandardBehaviourLibraryOld<CustomBehaviour> library = new StandardBehaviourLibraryOld<CustomBehaviour>();
        #endregion


        [ContextMenu("Test")]
        public void Setup() 
        { 
            bhv = FindObjectsOfType<CustomBehaviour>();
            library.InitializeLibrary(bhv);
        }
        



    }
}
