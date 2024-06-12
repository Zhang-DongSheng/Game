using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class RankingListView : ViewBase
    {
        [SerializeField] private ItemToggleGroup m_menu;

        [SerializeField] private List<UIRankinglistBase> views;

        protected override void OnAwake()
        {
            m_menu.callback = OnClickTab;

            m_menu.Refresh(0, 1, 2, 3, 4, 5);
        }

        public override void Refresh(UIParameter parameter)
        {
            m_menu.Select(0, true);
        }

        private void OnClickTab(int index)
        { 
            
        }
    }
}