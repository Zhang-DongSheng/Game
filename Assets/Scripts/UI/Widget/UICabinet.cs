using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UICabinet : ItemBase
    {
        [SerializeField] private RectTransform rect;

        [SerializeField] private List<ItemDrawer> items;

        [SerializeField] private bool active;

        private void Awake()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null)
                {
                    items[i].callback = Rebuilder;
                }
            }
        }

        public void Refresh()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null)
                {
                    items[i].Refresh(active);
                }
            }
        }

        public void SetActive(int index, bool active)
        {
            if (items.Count > index && items[index] != null)
            {
                items[index].SetActive(active);
            }
        }

        private void Rebuilder()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        }
    }
}