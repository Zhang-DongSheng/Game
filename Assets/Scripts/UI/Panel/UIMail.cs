using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIMail : UIBase
    {
        private readonly List<int> list = new List<int>();

        private void Start()
        {
            list.Clear();

            for (int i = 0; i < 30; i++)
            {
                list.Add(i);
            }
        }
    }
}
