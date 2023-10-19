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
            int count = Mathf.Clamp(2, 0, currencies.Count);

            for (int i = 0; i < count; i++)
            {
                currencies[i].Refresh();
            }
            for (int i = count; i < currencies.Count; i++)
            {
                currencies[i].SetActive(false);
            }
        }

        private void OnClickSetting()
        {
            UIQuickEntry.Open(UIPanel.UISetting);
        }
    }
}