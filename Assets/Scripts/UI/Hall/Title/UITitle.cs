using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITitle : UIBase
    {
        [SerializeField] private Button btnBack;

        [SerializeField] private Button btnShop;

        [SerializeField] private Button btnSetting;

        protected override void OnAwake()
        {
            EventManager.Register(EventKey.OpenPanel, Refresh);

            btnBack.onClick.AddListener(OnClickBack);

            btnShop.onClick.AddListener(OnClickShop);

            btnSetting.onClick.AddListener(OnClickSetting);
        }

        protected override void OnRelease()
        {
            EventManager.Unregister(EventKey.OpenPanel, Refresh);
        }

        public override void Refresh(UIParameter parameter)
        {
            
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

            Default();

            switch (information.panel)
            {
                case UIPanel.UIReward:
                    { 
                        // ȫ������
                    }
                    break;
                case UIPanel.UIMain:
                    {
                        btnShop.SetActive(true);

                        btnSetting.SetActive(true);
                    }
                    break;
                case UIPanel.UIShop:
                    {
                        btnBack.SetActive(true);
                    }
                    break;
                default:
                    {
                        btnShop.SetActive(true);

                        btnBack.SetActive(true);
                    }
                    break;
            }
        }

        private void Default()
        {
            btnShop.SetActive(false);

            btnBack.SetActive(false);

            btnSetting.SetActive(false);
        }

        private void OnClickShop()
        {
            UIQuickEntry.Open(UIPanel.UIShop);
        }

        private void OnClickSetting()
        {
            UIQuickEntry.Open(UIPanel.UISetting);
        }

        private void OnClickBack()
        {
            UIManager.Instance.Back();
        }
    }
}