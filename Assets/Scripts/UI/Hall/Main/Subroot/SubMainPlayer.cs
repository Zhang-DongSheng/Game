using Game.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SubMainPlayer : ItemBase
    {
        [SerializeField] private ItemAvatar m_avatar;

        [SerializeField] private ItemNickname m_nick;

        protected override void OnAwake()
        {
            m_avatar.onClick.AddListener(OnClick);
        }

        public void Refresh()
        {
            var player = PlayerLogic.Instance.Player;

            m_avatar.Refresh(player.head, player.frame);

            m_nick.Refresh(player.name);
        }

        private void OnClick()
        {
            UIQuickEntry.Open(UIPanel.PersonalInformation);
        }
    }
}
