using Game.UI;

namespace Game.State
{
    /// <summary>
    /// 登录账号
    /// </summary>
    public class GameStateLogin : IGameState
    {
        public void OnCreate()
        {

        }

        public void OnEnter()
        {
            UIQuickEntry.OpenSingle(UIPanel.Login, callback: LoadingView.Instance.Close);
        }

        public void OnStay()
        {

        }

        public void OnExit()
        {

        }
    }
}