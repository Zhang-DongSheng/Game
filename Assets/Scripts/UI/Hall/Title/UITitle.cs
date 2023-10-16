using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITitle : UIBase
    {
        [SerializeField] private Button player;

        [SerializeField] private Button back;

        [SerializeField] private Text title;

        [SerializeField] private Button setting;

        [SerializeField] private List<ItemCurrency> currencies;

        protected override void OnAwake()
        {
            player.onClick.AddListener(OnClickPlayer);

            back.onClick.AddListener(OnClickBack);

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

            RefreshCurrencies(information);

            if (information.type != UIType.Panel) return;

            title.text = information.panel.ToString();

            bool main = information.panel == UIPanel.UIMain;

            SetActive(player, main);

            SetActive(setting, main);

            SetActive(back, !main);
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

        private void OnClickPlayer()
        {
            UIQuickEntry.Open(UIPanel.UIPlayer);
        }

        private void OnClickBack()
        {
            UIManager.Instance.Back();
        }

        private void OnClickSetting()
        {
            UIQuickEntry.Open(UIPanel.UISetting);
        }
    }
}