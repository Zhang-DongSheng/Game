using Game.State;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITitle : UIBase
    {
        [SerializeField] private Text title;

        [SerializeField] private Button button;

        [SerializeField] private List<ItemCurrency> currencies;

        private UIPanel display;

        protected override void OnAwake()
        {
            button.onClick.AddListener(OnClick);
        }

        public override void Refresh(UIParameter parameter)
        {
            display = (UIPanel)parameter["panel"];

            title.text = display.ToString();

            RefreshCurrencies();

            bool active = GameStateController.Instance.current is GameLobbyState;

            if (active)
            {
                active = display != UIPanel.UIMain;
            }
            SetActive(active);
        }

        private void RefreshCurrencies()
        {
            var list = new List<uint>() { 101, 102, 103 };

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
            if (display == UIPanel.UIShop)
            {
                for (int i = 0; i < count; i++)
                {
                    currencies[i].HiddenSource();
                }
            }
        }

        private void OnClick()
        {
            UIManager.Instance.Back();
        }
    }
}