using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tag
{
    public class TabHandler : CustomBehaviour
    {
        public BaseView[] views;


        public void ShowTab(int index)
        {
            HideAllViews();
            views[index].ShowView();
        }


        private void HideAllViews()
        {
            for (int i = 0; i < views.Length; i++)
            {
                views[i].HideView();
            }
        }

    }
}
