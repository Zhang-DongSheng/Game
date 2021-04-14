using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Example.Scroll.Infinite
{
    public class Scroll : MonoBehaviour
    {
        [SerializeField] private InfiniteScrollList scroll;

        private void Start()
        {
            List<string> list = new List<string>();

            for (int i = 0; i < 100; i++)
            {
                list.Add(i.ToString());
            }
            scroll.Refresh(list);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
