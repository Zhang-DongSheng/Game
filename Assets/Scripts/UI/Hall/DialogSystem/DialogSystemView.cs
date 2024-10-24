using Game.Attribute;
using UnityEngine;

namespace Game.UI
{
    public class DialogSystemView : ViewBase
    {
        [SerializeField] private SubDialogSystemMenu _menu;
        [Display("╫ги╚")]
        [SerializeField] private SubDialogSystemPlayer _player;
        [Display("нд╠╬©Р")]
        [SerializeField] private SubDialogSystemTextbox _textbox;

        protected override void OnAwake()
        {
            _menu.onClickShowOrHide = OnClickShowOrHide;

            _menu.onClickNext = OnClickNext;

            _menu.onClickSkip = OnClickSkip;
        }

        public override void Refresh(UIParameter paramter)
        {
            var display = DialogSystemLogic.Instance.Display;

            _menu.RefreshDisplay(display);

            DialogSystemLogic.Instance.Refresh();
        }

        private void OnClickNext()
        {
            var dialog = DialogSystemLogic.Instance.Next();

            if (dialog == null) return;

            _player.RefreshState(dialog.role);

            _textbox.Refresh(dialog.content);
        }

        private void OnClickSkip()
        {
            _textbox.OnClickSkip();
        }

        private void OnClickShowOrHide()
        {
            var display = !DialogSystemLogic.Instance.Display;

            DialogSystemLogic.Instance.Display = display;

            _menu.RefreshDisplay(display);

            _player.RefreshDisplay(display);

            _textbox.RefreshDisplay(display);
        }
    }
}