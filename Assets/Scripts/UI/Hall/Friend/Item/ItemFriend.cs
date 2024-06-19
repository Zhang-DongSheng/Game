using UnityEngine;

namespace Game.UI
{
    public class ItemFriend : ItemBase
    {
        [SerializeField] private ItemAvatar m_avatar;

        [SerializeField] private ItemNickname m_nick;

        [SerializeField] private ItemStatus m_status;

        public void Refresh(Friend friend)
        {
            m_avatar.Refresh(friend.head, friend.frame);

            m_nick.Refresh(friend.name);

            Status status;

            switch (friend.relationship)
            {
                case 0:
                    status = Status.Available;
                    break;
                case -1:
                    status = Status.Undone;
                    break;
                default:
                    status = Status.Claimed;
                    break;

            }
            m_status.Refresh(status);

            SetActive(true);
        }
    }
}