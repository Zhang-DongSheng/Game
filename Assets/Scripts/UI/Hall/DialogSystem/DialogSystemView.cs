using Game.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class DialogSystemView : ViewBase
    {
        [SerializeField] private DialogSystemMenu menu;

        [SerializeField] private DialogSystemOption option;

        [SerializeField] private DialogSystemContent content;

        [SerializeField] private List<ItemDialogSystemPlayer> players;

        private bool display;

        protected override void OnAwake()
        {
            menu.btnHide.onClick.AddListener(OnClickShowOrHide);

            menu.btnNext.onClick.AddListener(OnClickNext);

            menu.btnSkip.onClick.AddListener(OnClickSkip);

            menu.btnBack.onClick.AddListener(OnClickClose);
        }

        public override void Refresh(UIParameter paramter)
        {
            display = true;

            menu.RefreshState(display);

            DialogSystemLogic.Instance.Refresh(Next);
        }

        private void Next()
        {
            var roles = DialogSystemLogic.Instance.Roles;

            var dialog = DialogSystemLogic.Instance.Next();

            if (dialog != null)
            {
                Refresh(dialog, roles);
            }
            else
            {
                Complete();
            }
        }

        private void Refresh(DialogInformation dialog, List<DialogRoleInformation> roles)
        {
            int count = players.Count;

            for (int i = 0; i < count; i++)
            {
                var role = roles.Find(x => x.position == players[i].position);

                if (role != null)
                {
                    players[i].Refresh(role, dialog.role);
                }
                else
                {
                    players[i].SetActive(false);
                }
            }

            switch (dialog.type)
            {
                case DialogType.Content:
                    {
                        content.Refresh(dialog);
                    }
                    break;
                case DialogType.Option:
                    {
                        option.Refresh(dialog);
                    }
                    break;
                default:
                    Debuger.LogError(Author.UI, $"该类型对话未处理:{dialog.type}");
                    break;
            }
            Debuger.LogWarning(Author.UI, $"当前播放对话ID:{dialog.primary}");
        }

        private void Complete()
        {
            Debuger.LogError(Author.UI, "对话完成");
        }

        private void OnClickNext()
        {
            Next();
        }

        private void OnClickSkip()
        {
            content.Skip();
        }

        private void OnClickShowOrHide()
        {
            display = !display;

            menu.RefreshState(display);

            int count = players.Count;

            for (int i = 0; i < count; i++)
            {
                players[i].ShowOrHide(display);
            }
            content.ShowOrHide(display);
        }
    }
}