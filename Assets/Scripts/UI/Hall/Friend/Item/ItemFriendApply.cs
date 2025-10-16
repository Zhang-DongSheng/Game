using Game.Data;
using UnityEngine;

namespace Game.UI
{
    public class ItemFriendApply : ItemBase
    {
        [SerializeField] private ItemAvatar m_avatar;

        [SerializeField] private ItemNickname m_nick;

        public void Refresh(Friend friend)
        {
            m_avatar.Refresh(friend.head, friend.frame);

            m_nick.Refresh(friend.name);

            SetActive(true);
        }
    }
}
