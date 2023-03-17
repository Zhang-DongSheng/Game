using Data;
using UnityEngine;

namespace Game.UI
{
    public class UIWarehouse : UIBase
    {
        [SerializeField] private UIMenu m_menu;

        [SerializeField] private UIWarehouseContent m_content;

        [SerializeField] private UIWarehouseIntroduce m_introduce;

        private void Awake()
        {
            m_menu.callback = OnClickTab;

            m_content.callback = OnClickProp;
        }

        private void Start()
        {
            m_menu.Initialize(3);

            m_menu.Select(0, true);
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