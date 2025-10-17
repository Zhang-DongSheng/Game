using Game.Logic;
using UnityEngine;

namespace Game.UI
{
    public class PersonalInformationView : ViewBase
    {
        [SerializeField] private ItemAvatar m_avatar;

        [SerializeField] private ItemNickname m_nick;

        protected override void OnAwake()
        {
            m_avatar.onClick.AddListener(OnClickPersonalization);
        }

        public override void Refresh(UIParameter paramter)
        {
            var player = PlayerLogic.Instance.Player;

            m_avatar.Refresh(player.head, player.frame);

            m_nick.Refresh(player.name);
        }

        private void OnClickPersonalization()
        {
            UIQuickEntry.Open(UIPanel.Personalization);
        }
    }
}