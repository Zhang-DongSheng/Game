using Game.Data;
using Game.Logic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class SubPersonalizationHead : SubPersonalizationBase
    {
        [SerializeField] private PrefabTemplateComponent prefab;

        private uint avatarID;

        private readonly List<ItemPersonalizationAvatar> items = new List<ItemPersonalizationAvatar>();

        public override void Refresh()
        {
            this.avatarID = PlayerLogic.Instance.Player.head;

            var list = DataManager.Instance.Load<DataAvatar>().list.FindAll(a => a.type == 1);

            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    var item = prefab.Create<ItemPersonalizationAvatar>();
                    item.callback = OnClickAvatar;
                    items.Add(item);
                }
                items[i].Refresh(list[i]);
            }
            OnClickAvatar(avatarID);
        }

        private void OnClickAvatar(uint avatarID)
        {
            this.avatarID = avatarID;

            int count = items.Count;

            for (int i = 0; i < count; i++)
            {
                items[i].Select(avatarID);
            }
            callback?.Invoke(avatarID);
        }

        public override PersonalizationType Type => PersonalizationType.Head;
    }
}