using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// �齱
    /// </summary>
    public class SubActivityLotteryDraw : SubActivityBase
    {
        [SerializeField] private AnimationListener animator;

        [SerializeField] private Button button;

        protected override void OnAwake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void DisplayRewards()
        {
            UIQuickEntry.OpenHorseLampView("�յ���XXXXX,ף�����YYYYY");
        }

        private void OnClick()
        {
            animator.Play(DisplayRewards);
        }
    }
}