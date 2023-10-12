using Game.State;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIMain : UIBase
    {
        [SerializeField] private Button btnWarehouse;

        [SerializeField] private Button btnMail;

        [SerializeField] private Button btnNew;

        [SerializeField] private Button btnCombat;

        [SerializeField] private Button btnActivity;

        [SerializeField] private Button btnLotteryDraw;

        protected override void OnAwake()
        {
            btnWarehouse.onClick.AddListener(OnClickWarehouse);

            btnMail.onClick.AddListener(OnClickMail);

            btnNew.onClick.AddListener(OnClickNew);

            btnCombat.onClick.AddListener(OnClickCombat);

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

        private void OnClickCombat()
        {
            GameStateController.Instance.EnterState<GameCombatState>();
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