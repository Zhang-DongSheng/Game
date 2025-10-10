using Game.UI;

namespace Game.State
{
    /// <summary>
    /// 游戏结算
    /// </summary>
    public class GameStateSettlement : IGameState
    {
        public void OnCreate()
        {

        }

        public void OnEnter()
        {
            UIQuickEntry.Open(UIPanel.Settlement);
        }

        public void OnExit()
        {

        }

        public void OnStay()
        {

        }
    }
}
