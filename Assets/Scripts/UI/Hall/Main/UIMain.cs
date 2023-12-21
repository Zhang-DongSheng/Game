using Game.State;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIMain : UIBase
    {
        [SerializeField] private Button combat;

        [SerializeField] private List<ItemEntry> entries;

        protected override void OnAwake()
        {
            combat.onClick.AddListener(OnClickCombat);
        }

        public override void Refresh(UIParameter parameter)
        {
            foreach (var entry in entries)
            {
                entry.Refresh();
            }
        }

        private void OnClickCombat()
        {
            GameStateController.Instance.EnterState<GameCombatState>();
        }
    }
}