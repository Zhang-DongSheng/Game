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

        [SerializeField] private PrefabTemplateComponent prefab;

        [SerializeField] private Button button;

        [SerializeField] private List<SubPersonalizationBase> subPersonalizations;

        private PersonalizationType current;

        private readonly List<ItemPersonalizationToggle> toggles = new List<ItemPersonalizationToggle>();

        protected override void OnAwake()
        {
            int count = subPersonalizations.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= toggles.Count)
                {
                    toggles.Add(prefab.Create<ItemPersonalizationToggle>());
                }
                toggles[i].Refresh(new ToggleParameter()
                {
                    index = (int)subPersonalizations[i].Type,
                    name = subPersonalizations[i].Type.ToString(),
                    callback = OnClickToggle,
                });
            }
            button.onClick.AddListener(OnClickConfirm);
        }

        protected override void OnRegister()
        {
            int count = subPersonalizations.Count;

            for (int i = 0; i < count; i++)
            {
                subPersonalizations[i].callback = OnClickPersonalization;
            }
        }

        public override void Refresh(UIParameter paramter)
        {
            RefreshPlayerInformation(PlayerLogic.Instance.Player);

            int count = subPersonalizations.Count;

            for (int i = 0; i < count; i++)
            {
                subPersonalizations[i].Refresh();
            }
            OnClickToggle(0);
        }

        public void RefreshPlayerInformation(Player player)
        { 
            m_avatar.Refresh(player.head, player.frame);

            m_nick.Refresh(player.name);
        }

        private void OnClickToggle(int index)
        {
            current = (PersonalizationType)index;

            int count = subPersonalizations.Count;

            for (int i = 0; i < count; i++)
            {
                toggles[i].Select(index);

                subPersonalizations[i].Switch(current);
            }
            // Refresh status
            switch (current)
            {
                case PersonalizationType.Head:
                    OnClickPersonalization(PlayerLogic.Instance.Cache.head);
                    break;
                case PersonalizationType.Frame:
                    OnClickPersonalization(PlayerLogic.Instance.Cache.frame);
                    break;
                case PersonalizationType.Nickname:
                    OnClickPersonalization(0);
                    break;
                case PersonalizationType.Country:
                    OnClickPersonalization(PlayerLogic.Instance.Cache.country);
                    break;
            }
        }

        private void OnClickPersonalization(uint ID)
        {
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
                    PlayerLogic.Instance.SetNickname(PlayerLogic.Instance.Cache.name, out status);
                    break;
                case PersonalizationType.Country:
                    PlayerLogic.Instance.SetCountry(ID, out status);
                    break;
            }
            m_status.Refresh(status);

            RefreshPlayerInformation(PlayerLogic.Instance.Cache);
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
        }
    }
}