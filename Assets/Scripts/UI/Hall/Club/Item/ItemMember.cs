using UnityEngine;

namespace Game.UI
{
    public class ItemMember : ItemBase
    {
        [SerializeField] private ItemAvatar m_avatar;

        [SerializeField] private ItemNickname m_nick;

        public void Refresh(Member member)
        {
            m_avatar.Refresh(member.head, member.frame);

            m_nick.Refresh(member.name);
        }
    }
}