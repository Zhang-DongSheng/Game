using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIMainPlayer : ItemBase
    {
        [SerializeField] private ItemPlayerAvatar avatar;

        [SerializeField] private Text nickname;

        [SerializeField] private Text level;

        [SerializeField] private Button button;

        protected override void OnAwake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            UIQuickEntry.Open(UIPanel.UIPlayer);
        }
    }
}
