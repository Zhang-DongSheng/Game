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
            EventManager.Register(EventKey.OpenPanel, Refresh);

            btnBack.onClick.AddListener(OnClickBack);

            btnShop.onClick.AddListener(OnClickShop);
        }

        private void OnDestroy()
        {
            EventManager.Unregister(EventKey.OpenPanel, Refresh);
        }

        private void Start()
        {
            if (UIManager.Instance.current != null)
            {
                Refresh(UIManager.Instance.current.information);
            }
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
                case UIPanel.UIMain:
                    {
                        btnShop.SetActive(true);
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
            Debuger.Log(Author.UI, information.panel.ToString());
        }

        private void Default()
        {
            btnShop.SetActive(false);

            btnBack.SetActive(false);
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