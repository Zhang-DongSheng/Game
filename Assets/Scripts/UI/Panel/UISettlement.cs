using Game.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UISettlement : UIBase
    {
        protected override void OnUpdate(float delta)
        {
            
        }

        public override void Refresh(UIParameter paramter)
        {
            
        }

        public override bool Back()
        {
            GameStateController.Instance.EnterState<GameLobbyState>();

            return base.Back();
        }
    }
}