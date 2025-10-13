using Game.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemPersonalizationAvatar : ItemBase
    {
        public Action<uint> callback;

        [SerializeField] private ImageBind imgAvatar;

        [SerializeField] private ItemStatus m_status;

        [SerializeField] private GameObject selected;

        [SerializeField] private Button button;

        private uint avatarID;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Refresh(AvatarInformation avatar)
        {
            avatarID = avatar.primary;

            imgAvatar.SetSprite(avatar.icon);
        }

        public void Select(uint avatarID)
        {
            bool active = this.avatarID == avatarID;

            SetActive(selected, active);
        }

        private void OnClick()
        {
            callback?.Invoke(avatarID);
        }
    }
}