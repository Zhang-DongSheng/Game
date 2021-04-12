using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class InfiniteScrollList : MonoBehaviour
    {
        [SerializeField] private InfiniteLayout content;

        [SerializeField] private RectTransform viewPort;

        [SerializeField] private GameObject prefab;

        [SerializeField] private int count = 10;

        private IList source;

        private readonly List<InfiniteItem> items = new List<InfiniteItem>();

        public void Initialize()
        {
            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    InfiniteItem item = GameObject.Instantiate(prefab, content.transform).GetComponent<InfiniteItem>();
                    items.Add(item);
                }
            }
        }

        public void Refresh(IList source)
        {
            this.source = source;
        }
    }
}