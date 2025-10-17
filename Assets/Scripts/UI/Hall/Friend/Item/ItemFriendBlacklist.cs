using Game.Data;
using Game.Logic;
using UnityEngine;

namespace Game.UI
{
    public class ItemFriendBlacklist : ItemBase
    {
        [SerializeField] private ItemAvatar m_avatar;

        [SerializeField] private ItemNickname m_nick;

        private uint uid;

        protected override void OnAwake()
        {
            m_avatar.onClick.AddListener(OnClickRemoveBlacklist);
        }

        public void Refresh(Friend friend)
        {
            uid = friend.uid;

            m_avatar.Refresh(friend.head, friend.frame);

            m_nick.Refresh(friend.name);

            SetActive(true);
        }

        private void OnClickRemoveBlacklist()
        {
            var component = GetComponent<Game.SM.SMRandomWalk>();

            component.enabled = false;

            UIQuickEntry.OpenConfirmView("Tips", "Confirm removal from the blacklist!", () =>
            {
                FriendLogic.Instance.RequestFriendRemoveBlacklist(uid);

                component.enabled = true;
            },()=>
            {
                component.enabled = true;
            });
        }
    }
}