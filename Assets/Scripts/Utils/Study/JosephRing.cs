using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Study
{
    /// <summary>
    /// 约瑟夫环[有死循环未解决]
    /// </summary>
    public class JosephRing : MonoBehaviour
    {
        private readonly List<int> people = new List<int>();

        private void Start()
        {
            for (int i = 0; i < 41; i++)
            {
                people.Add(i + 1);
            }
            Compute(people, 3);
        }

        private void Compute(List<int> list, int circle = 3)
        {
            int index = 0, step = 0;

            while (true)
            {
                if (list.Count < 3)
                {
                    break;
                }

                if (++step == circle)
                {
                    step = 1;

                    Debuger.LogError(Author.Test, "移除：" + list[index]);

                    list.RemoveAt(index);
                }

                if (index >= list.Count)
                {
                    index = 0;
                }

                if (++index >= list.Count)
                {
                    index = 0;
                }
            }
            Debuger.LogError(Author.Test, "剩余：" + list[0] + " and " + list[1]);
        }
    }
}
