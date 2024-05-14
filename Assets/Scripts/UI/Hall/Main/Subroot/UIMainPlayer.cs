using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIMainPlayer : ItemBase
    {
        [SerializeField] private ItemAvatar avatar;

        [SerializeField] private ItemNickname nick;

        [SerializeField] private Text level;

        [SerializeField] private Button button;

        protected override void OnAwake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Refresh()
        {
            //nickname.SetText()
        }

        private void OnClick()
        {
            UIQuickEntry.Open(UIPanel.Player);
        }
    }
}
