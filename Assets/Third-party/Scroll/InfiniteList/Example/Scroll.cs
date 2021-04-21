using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Example.Scroll.Infinite
{
    public class Scroll : MonoBehaviour
    {
        [SerializeField] private int count;

        [SerializeField] private InfiniteScrollList scroll;

        private readonly List<string> list = new List<string>();

        private void Start()
        {
            Refresh();
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            if (list.Count == count) return;

            list.Clear();

            for (int i = 0; i < count; i++)
            {
                list.Add(i.ToString());
            }
            scroll.Refresh(list);
        }
    }
}