using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tag
{

    public interface IHierarchyRefresher
    { 
        public void RegisterHierarchyRefresherAction(Action action);
    }


    public class ItemFactory :CustomBehaviour, IHierarchyRefresher
    {
        #region PUBLIC_VARS
        private List<Action> _onHierarchyRefresh = new List<Action>();
        public GameObject itemPrefab;

        public void RegisterHierarchyRefresherAction(Action action)
        {
            _onHierarchyRefresh.Add(action);
        }

        public T[] GenerateItems<T>(int count,Transform parentTransform){

            T[] items = new T[count];

            for (int i = 0; i < items.Length; i++)
            {
                items[i]=Instantiate(itemPrefab, parentTransform).GetComponent<T>();
            }

            OnHierarchyRefresh();

            return items;
            
        }

        private void OnHierarchyRefresh() {
            for (int i = 0; i < _onHierarchyRefresh.Count; i++)
            {
                _onHierarchyRefresh[i]();
            }
        }
        #endregion

        #region PRIVATE_VARS

        #endregion

        #region UNITY_CALLBACKS

        #endregion

        #region PUBLIC_FUNCTIONS

        #endregion

        #region PRIVATE_FUNCTIONS

        #endregion

        #region CO-ROUTINES

        #endregion

        #region EVENT_HANDLERS

        #endregion

        #region UI_CALLBACKS       

        #endregion
    }
}
