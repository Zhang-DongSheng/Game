using Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIPsd : UIBase
    {
        [SerializeField] private DataPSD source;

        [SerializeField] private ParentAndPrefab prefab;

        private readonly List<ItemPsd> items = new List<ItemPsd>();

        public PSDInformation config { get; set; }

        private void Awake()
        {
            config = source.First;

            Refresh();
        }

        public void Refresh()
        {
            if (config == null) return;

            for (int i = 0; i < config.sprites.Count; i++)
            {
                if (i >= items.Count)
                {
                    items.Add(prefab.Create<ItemPsd>());
                }
                items[i].Refresh(config.sprites[i]);
            }
            for (int i = config.sprites.Count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }
    }
}