using System.Collections;
using System.Collections.Generic;
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
            UIManager.Instance.Open(UIPanel.UITitle, record: false);
        }

        private void OnClickWarehouse()
        {
            UIManager.Instance.Open(UIPanel.UIWarehouse);
        }

        private void OnClickMail()
        {
            UIManager.Instance.Open(UIPanel.UIMail);
        }

        private void OnClickActivity()
        {
            UIManager.Instance.Open(UIPanel.UIActivity);
        }

        private void OnClickLotteryDraw()
        {
            UIManager.Instance.Open(UIPanel.UILotteryDraw);
        }
    }
}
