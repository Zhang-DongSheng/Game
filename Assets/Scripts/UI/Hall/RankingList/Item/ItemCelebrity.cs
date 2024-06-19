using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemCelebrity : ItemBase
    {
        [SerializeField] private ItemAvatar m_avatar;

        [SerializeField] private ItemNickname m_nick;

        [SerializeField] private Text m_rank;

        public void Refresh(Celebrity celebrity)
        {
            m_avatar.Refresh(celebrity.head, celebrity.frame);

            m_nick.Refresh(celebrity.name);

            m_rank.SetText(celebrity.rank);

            SetActive(true);
        }
    }
}