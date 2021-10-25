using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RandomWeighting
    {
        public static int Range<T>(List<T> list) where T : IWeight
        {
            int count = list.Count;

            float sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += list[i].Weight;
            }

            float value = Random.Range(0, sum);

            for (int i = 0; i < count; i++)
            {
                if (list[i].Weight > 0)
                {
                    value -= list[i].Weight;

                    if (value < 0)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}