using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;

namespace YondaimeFramework
{
	[CreateAssetMenu(fileName = "ComponentIdContainer", menuName = "YondaimeFramework/ComponentIdContainer")]
	public class SystemIdDataSO : ScriptableObject
	{
		[SerializeField] private ComponentIdSRC[] ids;

		public ComponentIdSRC[] GetIds()
		{
			return ids;
		}

        public void OnValidate()
        {
            for (int i = 0; i < ids.Length; i++)
            {
				ids[i].intValue = Animator.StringToHash(ids[i].stringIdVal);
		    }
        }
    }

	
}
