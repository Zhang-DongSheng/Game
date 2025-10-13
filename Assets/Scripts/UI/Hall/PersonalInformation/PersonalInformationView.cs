using UnityEngine;

namespace Game.UI
{
    public class PersonalInformationView : ViewBase
    {
        [SerializeField] private ItemAvatar m_avatar;

        [SerializeField] private ItemNickname m_nick;

        protected override void OnRegister()
        {
            m_avatar.onClick.AddListener(OnClickPersonalization);
        }

        public override void Refresh(UIParameter paramter)
        {

        }

        private void OnClickPersonalization()
        {
            UIQuickEntry.Open(UIPanel.Personalization);
        }
    }
}