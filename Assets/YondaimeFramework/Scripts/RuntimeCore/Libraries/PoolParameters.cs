using YondaimeFramework;
using UnityEngine;

namespace YondaimeFramework
{
    public class PoolParameters : MonoBehaviour
    {
        public PoolParameter[] parameters;

        [System.Serializable]
        public class PoolParameter
        {
            public CustomBehaviour Prefab;
            [Range(0,300)]public int PoolCount;
        }
    }
}
