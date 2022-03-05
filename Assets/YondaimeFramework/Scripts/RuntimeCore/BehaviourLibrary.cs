using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YondaimeFramework;


namespace YondaimeFramework
{
	public class BehaviourLibrary : MonoBehaviour
	{

		public const string COMPONENT_TAG = "libTag";


		[SerializeField] protected List<CustomBehaviour> _behaviours;
		[SerializeField] protected List<BehaviourLibrary> _childLibs;
		[SerializeField] protected PooledLibrary[] _childPooledLibs;


		

		public void AddBehaviour(CustomBehaviour behaviour) 
		{
			_behaviours.Add(behaviour);
		}
		
		public void AddLibrary(BehaviourLibrary behaviour) 
		{
			_childLibs.Add(behaviour);
		}

        internal void SetTag()
        {
			gameObject.tag = COMPONENT_TAG;
			_behaviours.Clear();
			_childLibs.Clear();
		}
	}
}