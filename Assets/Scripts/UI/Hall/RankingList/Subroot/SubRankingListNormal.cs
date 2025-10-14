using Game.Logic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class SubRankingListNormal : SubRankinglistBase
    {
        [SerializeField] private PrefabTemplateComponent prefab;

        private readonly List<ItemCelebrity> items = new List<ItemCelebrity>();

        public override void Refresh()
        {
            var list = RankingListLogic.Instance.GetRankings(index);

            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    items.Add(prefab.Create<ItemCelebrity>());
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
