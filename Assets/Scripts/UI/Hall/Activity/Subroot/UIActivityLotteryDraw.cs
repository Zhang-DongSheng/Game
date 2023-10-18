using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// �齱
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
            UIQuickEntry.OpenUIHorseLamp("�յ���XXXXX,ף�����YYYYY");
        }
    }
}