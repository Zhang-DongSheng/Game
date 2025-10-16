using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class FriendView : ViewBase
    {
        [SerializeField] private List<SubviewBase> m_views;

        [SerializeField] private ItemToggleGroup m_menu;

        private int index;

        protected override void OnAwake()
        {
            m_menu.Refresh(m_views);
        }

        protected override void OnRegister()
        {
            m_menu.callback = OnClickToggle;
        }

        public override void Refresh(UIParameter parameter)
        {
            var count = m_views.Count;

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

            var count = m_views.Count;

            for (int i = 0; i < count; i++)
            {
                m_views[i].Switch(index);
            }
        }
    }
}