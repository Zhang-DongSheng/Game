using Game.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class ActivityView : ViewBase
    {
        [SerializeField] private ItemToggleGroup m_menu;

        [SerializeField] private List<SubActivityBase> m_activities;

        protected override void OnAwake()
        {
            m_menu.callback = OnClickTab;
        }

        public override void Refresh(UIParameter parameter)
        {
            List<int> _activities = new List<int>();

            int count = m_activities.Count;

            for (int i = 0; i < count; i++)
            {
                if (ActivityLogic.Instance.IsOpen(m_activities[i].activityID))
                {
                    _activities.Add((int)m_activities[i].activityID);
                }
            }
            m_menu.Refresh(_activities.ToArray());

            if (_activities.Count > 0)
            {
                m_menu.Select(_activities[0], true);
            }
        }

        public void Refresh()
        {
            int count = m_activities.Count;

            for (int i = 0; i < count; i++)
            {
                if (ActivityLogic.Instance.IsOpen(m_activities[i].activityID))
                {
                    m_activities[i].Refresh();
                }
            }
        }

        private void OnClickTab(int index)
        {
            int count = m_activities.Count;

            for (int i = 0; i < count; i++)
            {
                m_activities[i].SetActive(m_activities[i].Equal((uint)index));
            }
        }
    }
}
