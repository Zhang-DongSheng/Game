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
        }

        public override void Refresh(UIParameter paramter)
        {
            bool shop = false;

            if (UIManager.Instance.TryGetCtrl(UIPanel.UIShop, out CtrlBase ctrl))
            {
                shop = ctrl.active;
            }
            btnShop.SetActive(!shop);
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