using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class SubFriends : SubviewBase
    {
        [SerializeField] private PrefabTemplateComponent prefab;

        private readonly List<ItemFriend> items = new List<ItemFriend>();
    }
}
