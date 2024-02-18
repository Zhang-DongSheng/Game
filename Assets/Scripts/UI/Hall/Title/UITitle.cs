using Game.State;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITitle : UIBase
    {
        [SerializeField] private TextBind title;

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

            switch (display)
            {
                case UIPanel.UIActivity:
                    {
                        title.SetText(display.ToString());

                        RefreshCurrencies(101, 102, 103);
                    }
                    break;
                default:
                    {
                        title.SetText(display.ToString());

                        RefreshCurrencies(101, 102, 103);
                    }
                    break;
            }
            RefreshDetection();

            bool active = 
                GameStateController.Instance.current is GameLobbyState ||
                GameStateController.Instance.current is GameDeployState;
            if (active)
            {
                active = display != UIPanel.UIMain;
            }
            SetActive(active);
        }

        private void RefreshCurrencies(params uint[] coins)
        {
            int count = Mathf.Clamp(coins.Length, 0, currencies.Count);

            for (int i = 0; i < count; i++)
            {
                currencies[i].Refresh(coins[i]);
            }
            for (int i = count; i < currencies.Count; i++)
            {
                currencies[i].SetActive(false);
            }
        }

        private void RefreshDetection()
        {
            if (display == UIPanel.UIShop)
            {
                int count = currencies.Count;

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