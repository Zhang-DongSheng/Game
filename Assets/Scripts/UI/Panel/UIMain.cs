using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIMain : UIBase
    {
        [SerializeField] private Button btnWarehouse;

        [SerializeField] private Button btnMail;

        [SerializeField] private Button btnNew;

        [SerializeField] private Button btnActivity;

        [SerializeField] private Button btnLotteryDraw;

        private void Awake()
        {
            btnWarehouse.onClick.AddListener(OnClickWarehouse);

            btnMail.onClick.AddListener(OnClickMail);

            btnNew.onClick.AddListener(OnClickNew);

            btnActivity.onClick.AddListener(OnClickActivity);

            btnLotteryDraw.onClick.AddListener(OnClickLotteryDraw);
        }

        private void Start()
        {
            UIQuickEntry.Open(UIPanel.UITitle);
        }

        private void OnClickWarehouse()
        {
            UIQuickEntry.Open(UIPanel.UIWarehouse);
        }

        private void OnClickMail()
        {
            UIQuickEntry.Open(UIPanel.UITest);
        }

        private void OnClickNew()
        {
            UIQuickEntry.OpenUITips("功能正在开发中");
        }

        private void OnClickActivity()
        {
            UIQuickEntry.Open(UIPanel.UIActivity);
        }

        private void OnClickLotteryDraw()
        {
            UIQuickEntry.Open(UIPanel.UILotteryDraw);
        }
    }
}