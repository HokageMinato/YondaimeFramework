using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;

namespace YondaimeFramework
{
    [CreateAssetMenu(fileName = "IdContainer", menuName = "YondaimeFramework/IdContainer", order = 1)]
    public class DrawerContentContainerSO : ScriptableObject
    {
        #region PUBLIC_VARS
        public string[] SystemIds => systemIds;
        #endregion

        #region PRIVATE_VARS
        [SerializeField] private string[] systemIds;
        #endregion


    }
}
