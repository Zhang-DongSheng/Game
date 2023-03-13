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

        protected override void OnDestroy()
        {
            EventManager.Unregister(EventKey.UIOpen, Refresh);

            EventManager.Unregister(EventKey.UIClose, Refresh);
        }

        protected override void OnUpdate(float delta)
        {
            
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

            btnBack.SetActive(UIManager.Instance.CanBack());
        }

        private void OnClickBack()
        {
            UIManager.Instance.Back();
        }

        private void OnClickShop()
        {
            UIQuickEntry.Open(UIPanel.UIShop);
        }
    }
}