using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIActivity : UIBase
    {
        [SerializeField] private ItemToggleGroup m_menu;

        [SerializeField] private List<UIActivityBase> m_activities;

        protected override void OnAwake()
        {
            m_menu.callback = OnClickTab;
        }

        public override void Refresh(UIParameter parameter)
        {
            m_menu.Initialize(3);

            m_menu.Select(0, true);
        }

        private void OnClickTab(int index)
        {
            Debug.LogError(index);
        }
    }
}
