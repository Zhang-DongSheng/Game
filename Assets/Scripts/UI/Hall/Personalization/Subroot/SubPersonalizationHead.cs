using Game.Data;
using Game.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class SubPersonalizationHead : SubPersonalizationBase
    {
        [SerializeField] private PrefabTemplateComponent prefab;

        private readonly List<ItemPersonalizationAvatar> items = new List<ItemPersonalizationAvatar>();

        public override void Refresh()
        {
            personalizatalID = PlayerLogic.Instance.Cache.head;

            var list = DataAvatar.GetList(1);

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
            OnClickAvatar(personalizatalID);
        }

        private void OnClickAvatar(uint avatarID)
        {
            personalizatalID = avatarID;

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