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

            m_menu.callback = OnClickToggle;
        }

        protected override void OnRegister()
        {
            EventDispatcher.Register(UIEvent.Friend, Refresh);
        }

        protected override void OnUnregister()
        {
            EventDispatcher.Unregister(UIEvent.Friend, Refresh);
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

        private void Refresh(EventArgs args)
        {
            var count = m_views.Count;

            for (int i = 0; i < count; i++)
            {
                m_views[i].Refresh();
            }
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