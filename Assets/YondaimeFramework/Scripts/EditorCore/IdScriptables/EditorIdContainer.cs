using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace YondaimeFramework.EditorHandles
{
	[CreateAssetMenu(fileName = "EditorIdContainer", menuName = "YondaimeFramework/EditorIdContainer")]
	public class EditorIdContainer : ScriptableObject
	{
		[SerializeField] private ComponentIdSRC[] ids;
		[SerializeField] private RuntimeIdContainer[] myRuntimeIdContainers;

		public ComponentIdSRC[] GetIdSRCs()
		{
			return ids;
		}


		public void UpdateRuntimeValues() 
		{

			foreach (var id in myRuntimeIdContainers) 
			{
				if (id != null)
				{
					id.SetIds(GenerateRuntimeIds());
					EditorUtility.SetDirty(id);
				}
				else 
				{
					throw new System.Exception($"Null RuntimeIdContainer entry in {name} editorIdContainer");
				}
			}	
			
			
		}

		
		
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
