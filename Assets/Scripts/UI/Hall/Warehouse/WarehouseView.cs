using UnityEngine;

namespace Game.UI
{
    public class WarehouseView : ViewBase
    {
        [SerializeField] private ItemToggleGroup m_menu;

        [SerializeField] private UIWarehouseContent m_content;

        [SerializeField] private UIWarehouseIntroduce m_introduce;

        protected override void OnAwake()
        {
            m_menu.callback = OnClickTab;

            m_menu.Refresh<WarehouseCategory>();

            m_content.callback = OnClickProp;
        }

        private void Start()
        {
            m_menu.Select((int)WarehouseCategory.Consume, true);
        }

        private void OnClickTab(int index)
        {
            m_content.Refresh(index);
        }

        private void OnClickProp(Prop prop)
        {
            m_introduce.Refresh(prop);
        }
    }
}