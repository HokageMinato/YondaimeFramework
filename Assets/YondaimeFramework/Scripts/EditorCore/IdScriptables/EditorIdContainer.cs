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


		[ContextMenu("UpdateRuntimeIds")]
		public void UpdateRuntimeValues() 
		{
			CheckForDuplicates();

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

		private void CheckForDuplicates() 
		{
			HashSet<string> duplicates = new HashSet<string>();
            foreach (var item in ids)
            {
				if (duplicates.Contains(item.stringIdVal))
					throw new System.Exception($"Duplicate id entry {item.stringIdVal} detected");	

				if(item.stringIdVal != string.Empty)
					duplicates.Add(item.stringIdVal);
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
