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
		[SerializeField] private ComponentId[] idParsed;

		public ComponentIdSRC[] GetIdSRCs()
		{
			return ids;
		}

		public ComponentId[] GetIds() 
		{
			return idParsed;
		}

		public void SetParsedValues(ComponentId[] parsed) 
		{
			idParsed = parsed;
		}

    }

	
}
