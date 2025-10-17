using Game.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ActivityView : ViewBase
    {
        [SerializeField] private ItemToggleGroup m_menu;

        [SerializeField] private List<SubviewBase> m_views;

        private int index;

        protected override void OnAwake()
        {
            m_menu.callback = OnClickToggle;
        }

        public override void Refresh(UIParameter parameter)
        {
            RefreshActivities();

            int count = m_views.Count;

            for (int i = 0; i < count; i++)
            {
                var activityID = (uint)m_views[i].subviewID;

                if (ActivityLogic.Instance.IsOpen(activityID))
                {
                    m_views[i].Refresh();
                }
            }
        }

        public void RefreshActivities()
        {
            var list = new List<int>();

            int count = m_views.Count;

            for (int i = 0; i < count; i++)
            {
                var activityID = m_views[i].subviewID;

                if (ActivityLogic.Instance.IsOpen((uint)activityID))
                {
                    list.Add(activityID);
                }
            }
            m_menu.Refresh(list.ToArray());

            index = list[0];

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