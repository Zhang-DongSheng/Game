using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITitle : UIBase
    {
        [SerializeField] private Button btnBack;

        [SerializeField] private Button btnShop;

        private void Awake()
        {
            btnBack.onClick.AddListener(OnClickBack);

            btnShop.onClick.AddListener(OnClickShop);

            EventManager.Register(EventKey.UIOpen, Refresh);

            EventManager.Register(EventKey.UIClose, Refresh);
        }

        private void OnDestroy()
        {
            EventManager.Unregister(EventKey.UIOpen, Refresh);

            EventManager.Unregister(EventKey.UIClose, Refresh);
        }

        public override void Refresh(Paramter paramter)
        {
            Refresh(null);
        }

        private void Refresh(EventMessageArgs args)
        {
            bool shop = false;

            if (UIManager.Instance.TryGetCtrl(UIPanel.UIShop, out CtrlBase ctrl))
            {
                shop = ctrl.active;
            }
            btnShop.SetActive(!shop);

            bool back = UIManager.Instance.OnlyOne(UIPanel.UIMain, UIPanel.UITitle);

            btnBack.SetActive(!back);
        }

        private void OnClickBack()
        {
            UIManager.Instance.Back();
        }

        private void OnClickShop()
        {
            UIManager.Instance.Open(UIPanel.UIShop);
        }
    }
}