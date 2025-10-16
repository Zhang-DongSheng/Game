using Game.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class SubFriends : SubviewBase
    {
        [SerializeField] private PrefabTemplateComponent prefab;

        private readonly List<ItemFriend> items = new List<ItemFriend>();

        public override void Refresh()
        {
            var list = FriendLogic.Instance.GetFriends(subviewID);

            var count = list.Count;

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
    }
}
