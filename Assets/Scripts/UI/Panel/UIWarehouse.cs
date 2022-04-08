using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIWarehouse : UIBase
    {
        [SerializeField] private TabGroup tab;

        [SerializeField] private UIWarehouseContent content;

        [SerializeField] private UIWarehouseIntroduce introduce;

        private void Awake()
        {
            tab.Initialize(OnClickTab);

            content.callback = OnClickProp;
        }

        private void Start()
        {
            tab.Refresh(new int[5] { 1, 2, 3, 4, 5 });
        }

        private void OnClickTab(int index)
        {
            content.Refresh(index);
        }

        private void OnClickProp(Prop prop)
        {
            introduce.Refresh(prop);
        }
    }
}