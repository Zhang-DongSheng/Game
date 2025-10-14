using Game.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class SubMainTitle : ItemBase
    {
        [SerializeField] private ItemAvatar m_avatar;

        [SerializeField] private ItemNickname m_nick;

        [SerializeField] private List<ItemCurrency> currencies;

        protected override void OnAwake()
        {
            m_avatar.onClick.AddListener(OnClick);
        }

        public void Refresh()
        {
            var player = PlayerLogic.Instance.Player;

            m_avatar.Refresh(player.head, player.frame);

            m_nick.Refresh(player.name);

            RefreshCurrencies();
        }

        public void RefreshCurrencies()
        {
            var list = new List<uint>() { 101, 102, 103 };

            int count = Mathf.Clamp(list.Count, 0, currencies.Count);

            for (int i = 0; i < count; i++)
            {
                currencies[i].Refresh(list[i]);
            }
            for (int i = count; i < currencies.Count; i++)
            {
                currencies[i].SetActive(false);
            }
        }

        private void OnClick()
        {
            UIQuickEntry.Open(UIPanel.PersonalInformation);
        }
    }
}
