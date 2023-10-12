using Game.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UISettlement : UIBase
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
            GameStateController.Instance.EnterState<GameLobbyState>();
        }

        public override bool Back()
        {
            OnClickClose(); return false;
        }
    }
}