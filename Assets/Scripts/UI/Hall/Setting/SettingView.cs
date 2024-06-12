using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class SettingView : ViewBase
    {
        [SerializeField] private ItemToggleGroup m_menu;

        [SerializeField] private List<UISettingBase> views;

        protected override void OnAwake()
        {
            m_menu.callback = OnClickTab;

            m_menu.Refresh(0, 1, 2, 3);
        }

        public override void Refresh(UIParameter parameter)
        {
            int count = views.Count;

            for (int i = 0; i < count; i++)
            {
                views[i].Refresh();
            }
            m_menu.Select(0, true);
        }

        private void OnClickTab(int index)
        {
            int count = views.Count;

            for (int i = 0; i < count; i++)
            {
                views[i].SetActive(i == index);
            }
        }
    }
}