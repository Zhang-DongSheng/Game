using Game.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class CombatReadinessView : ViewBase
    {
        [SerializeField] private Button btnCombat;

        protected override void OnAwake()
        {
            btnCombat.onClick.AddListener(OnClickCombat);
        }

        public override void Refresh(UIParameter paramter)
        {
            
        }

        private void OnClickCombat()
        {
            GameStateController.Instance.EnterState<GameCombatState>();
        }
    }
}