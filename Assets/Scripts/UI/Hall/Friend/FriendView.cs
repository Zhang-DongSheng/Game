using Game.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class FriendView : ViewBase
    {
        [SerializeField] private PrefabTemplateBehaviour prefab;

        [SerializeField] private ItemToggleGroup m_menu;

        private readonly List<ItemFriend> items = new List<ItemFriend>();

        protected override void OnAwake()
        {
            m_menu.callback = OnClickTab;

            m_menu.Refresh(0, 1, 2);
        }

        public override void Refresh(UIParameter parameter)
        {
            m_menu.Select(0);
        }

        private void RefreshFriends(int index)
        {
            var list = FriendLogic.Instance.GetFriends(index);

            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    items.Add(prefab.Create<ItemFriend>());
                }
                items[i].Refresh(list[i]);
            }
            for (int i = count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }

        private void OnClickTab(int index)
        {
            RefreshFriends(index);
        }
    }
}