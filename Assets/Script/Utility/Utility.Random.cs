using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        public static class Random
        {
            private static readonly List<int> sequence = new List<int>();

            public static int Next(int min, int max)
            {
                sequence.Clear();

                for (int i = min; i < max; i++)
                {
                    sequence.Add(i);
                }
                return Next();
            }

            public static int Next()
            {
                if (sequence.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, sequence.Count);

                    int result = sequence[index];

                    sequence.RemoveAt(index);

                    return result;
                }
                return -1;
            }

            public static int Range<T>(List<T> list) where T : IWeight
            {
                int count = list.Count;

                float step, sum = 0;

                for (int i = 0; i < count; i++)
                {
                    sum += list[i].value;
                }
                step = UnityEngine.Random.Range(0, sum);

                for (int i = 0; i < count; i++)
                {
                    if (list[i].value > 0)
                    {
                        step -= list[i].value;

                        if (step < 0)
                        {
                            return i;
                        }
                    }
                }
                return -1;
            }

            public static List<int> Shuffle(params int[] list)
            {
                sequence.Clear();

                if (list != null && list.Length > 0)
                {
                    int index, count = list.Length;

                    List<int> indices = new List<int>(count);

                    for (int i = 0; i < count; i++)
                    {
                        indices.Add(i);
                    }
                    while (count > 0)
                    {
                        index = UnityEngine.Random.Range(0, count--);

                        sequence.Add(list[indices[index]]);

                        indices.RemoveAt(index);
                    }
                }
                return sequence;
            }
        }
    }
}