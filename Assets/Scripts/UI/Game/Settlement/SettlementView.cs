using Game.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SettlementView : ViewBase
    {
        [SerializeField] private Button btnClose;

        protected override void OnAwake()
        {
            btnClose.onClick.AddListener(OnClickClose);
        }

        public override void Refresh(UIParameter paramter)
        {

        }

        protected override void OnClickClose()
        {
            GameStateController.Instance.EnterState<GameStateLobby>();
        }

        public override bool Back()
        {
            OnClickClose(); return false;
        }
    }
}