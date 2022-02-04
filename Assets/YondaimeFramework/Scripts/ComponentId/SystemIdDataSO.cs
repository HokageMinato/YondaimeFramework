using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;

[CreateAssetMenu(fileName = "ComponentIdContainer" , menuName = "YondaimeFramework/ComponentIdContainer")]
public class SystemIdDataSO : ScriptableObject
{
	[SerializeField] private string[] ids;

	public string[] GetIds() 
	{
		return ids;
	}
}