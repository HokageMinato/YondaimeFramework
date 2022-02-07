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

    }

	
}
