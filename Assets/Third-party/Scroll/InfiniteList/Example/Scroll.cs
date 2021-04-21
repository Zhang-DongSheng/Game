using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Example.Scroll.Infinite
{
    public class Scroll : MonoBehaviour
    {
        [SerializeField] private int count;

        [SerializeField] private InfiniteScrollList scroll;

        private void Start()
        {
            List<string> list = new List<string>();

            for (int i = 0; i < count; i++)
            {
                list.Add(i.ToString());
            }
            scroll.Refresh(list);
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                Start();
            }
        }
    }
}
