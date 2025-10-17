using Game.Data;
using Game.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemFriendApply : ItemBase
    {
        [SerializeField] private ItemAvatar m_avatar;

        [SerializeField] private ItemNickname m_nick;

        [SerializeField] private Button btnAgree;

        [SerializeField] private Button btnRefuse;

        private uint uid;

        protected override void OnAwake()
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
            FriendLogic.Instance.RequestFriendApply(uid, true);
        }

        private void OnClickRefuse()
        {
            FriendLogic.Instance.RequestFriendApply(uid, false);
        }
    }
}