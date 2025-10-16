using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 抽奖
    /// </summary>
    public class SubActivityLotteryDraw : SubviewBase
    {
        [SerializeField] private AnimationListener animator;

        [SerializeField] private Button button;

        protected override void OnAwake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void DisplayRewards()
        {
            UIQuickEntry.OpenHorseLampView("收到大奖XXXXX,祝贺玩家YYYYY");
        }

        private void OnClick()
        {
            animator.Play(DisplayRewards);
        }
    }
}