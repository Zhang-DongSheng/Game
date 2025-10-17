using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class SettingView : ViewBase
    {
        [SerializeField] private ItemToggleGroup m_menu;

        [SerializeField] private List<SubviewBase> m_views;

        private int index;

        protected override void OnAwake()
        {
            m_menu.Refresh(m_views);

            m_menu.callback = OnClickToggle;
        }

        public override void Refresh(UIParameter parameter)
        {
            int count = m_views.Count;

            for (int i = 0; i < count; i++)
            {
                m_views[i].Refresh();
            }
            index = m_views[0].subviewID;

            m_menu.Select(index, true);
        }

        private void OnClickToggle(int index)
        {
            this.index = index;

            int count = m_views.Count;

            for (int i = 0; i < count; i++)
            {
                m_views[i].Switch(index);
            }
        }
    }
}