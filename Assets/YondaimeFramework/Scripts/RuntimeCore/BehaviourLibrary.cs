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


		

		public void AddBehaviour(CustomBehaviour behaviour) 
		{
			_behaviours.Add(behaviour);
		}
		
		public void AddLibrary(BehaviourLibrary behaviour) 
		{
			_childLibs.Add(behaviour);
		}

        public void InitLibrary()
        {
			gameObject.tag = COMPONENT_TAG;
			_behaviours.Clear();
			_childLibs.Clear();
		}

		public Transform FindLibParentTransform() 
		{
			Transform t = transform.parent;

			while (t != null && t != transform && !t.CompareTag(COMPONENT_TAG))
			{

				t = t.parent;
			}


			if (t == null)
				return null;

			return t;
		}

	}
}