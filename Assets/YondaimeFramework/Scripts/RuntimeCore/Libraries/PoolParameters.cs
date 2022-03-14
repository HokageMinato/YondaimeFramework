using YondaimeFramework;
using UnityEngine;

namespace YondaimeFramework
{
    public class PoolParameters : CustomBehaviour
    {
        public PoolParameter[] parameters;

        public void GenerateInstances() 
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                PoolParameter parameter = parameters[i];
                for (int k = 0; k < parameter.PoolCount; k++)
                    Pool(Instantiate(parameter.Prefab));
            }
        }

    }
        [System.Serializable]
        public class PoolParameter
        {
            public CustomBehaviour Prefab;
            [Range(0,300)][SerializeField]private int poolCount;
            public int PoolCount {
             get { return poolCount; }
        }
        }
}
