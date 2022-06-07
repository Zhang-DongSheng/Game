using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIMain : UIBase
    {
        [SerializeField] private Button btnWarehouse;

        [SerializeField] private Button btnMail;

        [SerializeField] private Button btnActivity;

        [SerializeField] private Button btnLotteryDraw;

        private void Awake()
        {
            btnWarehouse.onClick.AddListener(OnClickWarehouse);

            btnMail.onClick.AddListener(OnClickMail);

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
            UIQuickEntry.Open(UIPanel.UIMail);
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