using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.State
{
    /// <summary>
    /// 游戏结算
    /// </summary>
    public class GameSettlementState : IGameState
    {
        public void OnCreate()
        {

        }

        public void OnEnter()
        {
            UIManager.Instance.Open(UIPanel.UISettlement);
        }

        public void OnExit()
        {

        }

        public void OnStay()
        {

        }
    }
}
