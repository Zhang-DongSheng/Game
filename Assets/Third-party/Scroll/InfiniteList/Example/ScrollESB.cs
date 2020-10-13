using Example;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    public class ScrollESB : MonoBehaviour
    {
        public Transform parent;

        public GameObject prefab;

        public ScrollList scroll;

        private readonly List<ItemESB> items = new List<ItemESB>();

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < 10; i++)
            {
                if (i >= items.Count)
                {
                    GameObject go = GameObject.Instantiate(prefab, parent);
                    items.Add(go.GetComponent<ItemESB>());
                }
                items[i].Refresh(i.ToString());
            }
            scroll.FormatPosition();
        }
    }
}
