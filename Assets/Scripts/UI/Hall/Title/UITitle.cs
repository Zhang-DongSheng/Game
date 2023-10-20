using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITitle : UIBase
    {
        [SerializeField] private UITitlePlayer player;

        [SerializeField] private UITitleBack back;

        [SerializeField] private Button setting;

        [SerializeField] private List<ItemCurrency> currencies;

        protected override void OnAwake()
        {
            setting.onClick.AddListener(OnClickSetting);
        }

        protected override void OnRegister()
        {
            EventManager.Register(EventKey.OpenPanel, Refresh);
        }

        protected override void OnUnregister()
        {
            EventManager.Unregister(EventKey.OpenPanel, Refresh);
        }

        public override void Refresh(UIParameter parameter)
        {
            Refresh(UIManager.Instance.Current.information);
        }

        private void Refresh(EventMessageArgs args)
        {
            if (args[0] is UIInformation information)
            {
                Refresh(information);
            }
        }

        private void Refresh(UIInformation information)
        {
            if (information == null) return;

            if (information.type != UIType.Panel) return;

            RefreshCurrencies(information);

            bool main = information.panel == UIPanel.UIMain;

            if (main)
            {
                player.Refresh(information);

                back.SetActive(false);
            }
            else
            {
                player.SetActive(false);

                back.Refresh(information);
            }
            SetActive(setting, main);
        }

        private void RefreshCurrencies(UIInformation information)
        {
            var list = new List<int>() { 101, 102, 103 };

            int count = Mathf.Clamp(list.Count, 0, currencies.Count);

            for (int i = 0; i < count; i++)
            {
                currencies[i].Refresh(list[i]);
            }
            for (int i = count; i < currencies.Count; i++)
            {
                currencies[i].SetActive(false);
            }
            // 特殊界面处理
            if (information.panel == UIPanel.UIShop)
            {
                for (int i = 0; i < count; i++)
                {
                    currencies[i].HiddenSource();
                }
            }
        }

        private void OnClickSetting()
        {
            UIQuickEntry.Open(UIPanel.UISetting);
        }
    }
}