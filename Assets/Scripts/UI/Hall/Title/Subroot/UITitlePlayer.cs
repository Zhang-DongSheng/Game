using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UITitlePlayer : ItemBase
    {
        [SerializeField] private ItemPlayerAvatar avatar;

        [SerializeField] private Text nickname;

        [SerializeField] private Text level;

        [SerializeField] private Button button;

        protected override void OnAwake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Refresh(UIInformation information)
        {
            SetActive(true);
        }

        private void OnClick()
        {
            UIQuickEntry.Open(UIPanel.UIPlayer);
        }
    }
}
