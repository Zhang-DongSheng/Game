using Game.Data;
using Game.Logic;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PersonalizationView : ViewBase
    {
        [SerializeField] private ItemAvatar m_avatar;

        [SerializeField] private ItemNickname m_nick;

        [SerializeField] private ItemStatus m_status;

        [SerializeField] private ItemToggleGroup m_menu;

        [SerializeField] private Button button;

        [SerializeField] private List<SubPersonalizationBase> subPersonalizations;

        private PersonalizationType current;

        protected override void OnAwake()
        {
            m_menu.Refresh<PersonalizationType>();
        }

        protected override void OnRegister()
        {
            m_menu.callback = OnClickToggle;

            int count = subPersonalizations.Count;

            for (int i = 0; i < count; i++)
            {
                subPersonalizations[i].callback = OnClickPersonalization;
            }
            button.onClick.AddListener(OnClickConfirm);
        }

        public override void Refresh(UIParameter paramter)
        {
            var player = PlayerLogic.Instance.Player;

            PlayerLogic.Instance.Cache.Copy(player);

            RefreshInformation(player, Status.Claimed);

            int count = subPersonalizations.Count;

            for (int i = 0; i < count; i++)
            {
                subPersonalizations[i].Refresh();
            }
            OnClickToggle(0);
        }

        public void RefreshInformation(Player player, Status status)
        { 
            m_avatar.Refresh(player.head, player.frame);

            m_nick.Refresh(player.name);

            m_status.Refresh(status);
        }

        public void RefreshPersonalization()
        {
            int count = subPersonalizations.Count;

            for (int i = 0; i < count; i++)
            {
                if (subPersonalizations[i].Type == current)
                {
                    subPersonalizations[i].Refresh();
                    break;
                }
            }
        }

        private void OnClickToggle(int index)
        {
            current = (PersonalizationType)index;

            int count = subPersonalizations.Count;

            for (int i = 0; i < count; i++)
            {
                subPersonalizations[i].Switch(current);
            }
        }

        private void OnClickPersonalization(uint ID)
        {
            var player = PlayerLogic.Instance.Cache;

            var status = Status.Undone;

            switch (current)
            {
                case PersonalizationType.Head:
                    PlayerLogic.Instance.SetHead(ID, out status);
                    break;
                case PersonalizationType.Frame:
                    PlayerLogic.Instance.SetFrame(ID, out status);
                    break;
                case PersonalizationType.Nickname:
                    PlayerLogic.Instance.SetNickname(player.name, out status);
                    break;
                case PersonalizationType.Country:
                    PlayerLogic.Instance.SetCountry(ID, out status);
                    break;
            }
            RefreshInformation(player, status);
        }

        private void OnClickConfirm()
        {
            switch (current)
            {
                case PersonalizationType.Head:
                    PlayerLogic.Instance.Player.head = PlayerLogic.Instance.Cache.head;
                    break;
                case PersonalizationType.Frame:
                    PlayerLogic.Instance.Player.frame = PlayerLogic.Instance.Cache.frame;
                    break;
                case PersonalizationType.Nickname:
                    PlayerLogic.Instance.Player.name = PlayerLogic.Instance.Cache.name;
                    break;
                case PersonalizationType.Country:
                    PlayerLogic.Instance.Player.country = PlayerLogic.Instance.Cache.country;
                    break;
            }
            RefreshPersonalization();
        }
    }
}