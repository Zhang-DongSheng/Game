using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class RankingListView : ViewBase
    {
        [SerializeField] private ItemToggleGroup m_menu;

        [SerializeField] private List<SubRankinglistBase> views;

        protected override void OnAwake()
        {
            m_menu.callback = OnClickTab;

            List<int> list = new List<int>();

            foreach (var view in views)
            {
                if (view != null && view.open)
                {
                    list.Add(view.index);
                }
            }
            m_menu.Refresh(list.ToArray());
        }

        public override void Refresh(UIParameter parameter)
        {
            m_menu.Select(0, true);

            int count = views.Count;

            for (int i = 0; i < count; i++)
            {
                if (views[i] != null && views[i].open)
                {
                    views[i].Refresh();
                }
            }
        }

        private void OnClickTab(int index)
        {
            int count = views.Count;

            for (int i = 0; i < count; i++)
            {
                if (views[i] != null)
                {
                    views[i].SetActive(views[i].index == index);
                }
            }
        }
    }
}