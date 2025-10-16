using Game.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemFriend : ItemBase
    {
        [SerializeField] private ItemAvatar m_avatar;

        [SerializeField] private ItemNickname m_nick;

        [SerializeField] private Button btnAgree;

        [SerializeField] private Button btnRefuse;

        protected override void OnRegister()
        {
            btnAgree.onClick.AddListener(OnClickAgree);

            btnRefuse.onClick.AddListener(OnClickRefuse);
        }

        public void Refresh(Friend friend)
        {
            m_avatar.Refresh(friend.head, friend.frame);

            m_nick.Refresh(friend.name);

            SetActive(true);
        }

        private void OnClickAgree()
        { 
            
        }

        private void OnClickRefuse()
        {

        }
    }
}