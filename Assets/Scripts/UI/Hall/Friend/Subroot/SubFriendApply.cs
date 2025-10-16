using Game.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class SubFriendApply : SubviewBase
    {
        [SerializeField] private List<ItemFriendApply> items;

        public override void Refresh()
        {
            var list = FriendLogic.Instance.GetFriends(subviewID);

            var count = Mathf.Clamp(list.Count, 0, items.Count);

            for (int i = 0; i < count; i++)
            {
                items[i].Refresh(list[i]);
            }
            for (int i = count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }
    }
}