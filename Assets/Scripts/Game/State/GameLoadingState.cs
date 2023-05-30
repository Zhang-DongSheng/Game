using Game.Resource;
using Game.UI;

namespace Game.State
{
    /// <summary>
    /// 资源加载
    /// </summary>
    public class GameLoadingState : IGameState
    {
        public void OnCreate()
        {

        }

        public void OnEnter()
        {
            UILoading.Instance.Open();
        }

        public void OnExit()
        {
            
        }

        public void OnStay()
        {

        }
    }
}
