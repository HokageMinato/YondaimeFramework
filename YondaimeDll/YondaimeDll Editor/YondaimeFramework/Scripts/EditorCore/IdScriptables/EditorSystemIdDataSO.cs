using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YondaimeFramework;

namespace YondaimeFramework.EditorHandles
{
	[CreateAssetMenu(fileName = "EditorSystemIdData", menuName = "YondaimeFramework/EditorSystemIdData")]
	public class EditorSystemIdDataSO : ScriptableObject
	{
		[SerializeField] private ComponentIdSRC[] ids;
		[SerializeField] private RuntimeIdContainer runtimeIdContainer;

		public ComponentIdSRC[] GetIdSRCs()
		{
			return ids;
		}

		public void UpdateRuntimeValues() 
		{ 
			runtimeIdContainer?.SetIds(GenerateRuntimeIds());
		}

		[ContextMenu("GenerateRuntimeIds")]
		public ComponentId[] GenerateRuntimeIds() 
		{
			List<ComponentId> componentId = new List<ComponentId>();

			foreach (var id in ids)
				componentId.Add(id.ParseToRuntimeId());


			EditorUtility.SetDirty(this);
			return componentId.ToArray();
	
		}


    }

	
}
