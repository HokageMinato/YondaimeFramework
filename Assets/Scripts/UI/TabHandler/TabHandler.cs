using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace YondaimeFramework
{
    public class TabHandler : CustomBehaviour
    {
        #region PUBLIC_VARS
        //public BaseView[] baseViews;
        private List<Action<int>> onSwitched = new List<Action<int>>();
        #endregion

        #region PRIVATE_VARS

        #endregion

        #region UNITY_CALLBACKS

        #endregion

        #region PUBLIC_FUNCTIONS
        public void Init()
        {
            RegisterTabSwitchers();
        }
        #endregion

        #region PRIVATE_FUNCTIONS
        private void RegisterTabSwitchers()
        {
            List<ITabHandlerSwitchCallback> tabSwitcher = GetComponentsFromLibrary<ITabHandlerSwitchCallback>();
            Debug.Log($"Got {tabSwitcher.Count}");
            for (int i = 0; i < tabSwitcher.Count; i++)
            {
                onSwitched.Add(tabSwitcher[i].OnTabSwitch);
            }
        }

        private void SwitchToTab(int index)
        {
            //HideAllTabs();
            // baseViews[index].ShowView();
            for (int i = 0; i < onSwitched.Count; i++)
            {
                onSwitched[i](index);
            }
        }

        private void HideAllTabs()
        {
            // for (int i = 0; i < baseViews.Length; i++)
            {
                //      baseViews[i].HideView();
            }
        }
        #endregion

        #region CO-ROUTINES

        #endregion

        #region EVENT_HANDLERS

        #endregion

        #region UI_CALLBACKS       
        public void OnTabButtonClicked(int tabIndnex)
        {
            SwitchToTab(tabIndnex);
        }
        #endregion

    }

    public interface ITabHandlerSwitchCallback
    {
        public void OnTabSwitch(int switchedToId);
    }


}
