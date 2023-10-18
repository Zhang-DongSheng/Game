using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 抽奖
    /// </summary>
    public class UIActivityLotteryDraw : UIActivityBase
    {
        [SerializeField] private Button button;

        protected override void OnAwake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            UIQuickEntry.OpenUIHorseLamp("收到大奖XXXXX,祝贺玩家YYYYY");
        }
    }
}