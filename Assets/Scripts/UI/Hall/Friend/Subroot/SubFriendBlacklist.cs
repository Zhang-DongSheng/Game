using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class SubFriendBlacklist : SubviewBase
    {
        [SerializeField] private PrefabTemplateComponent prefab;

        private readonly List<ItemFriend> items = new List<ItemFriend>();
    }
}
