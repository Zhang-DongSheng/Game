using Game;
using System.Collections.Generic;

namespace Data
{
    [System.Serializable]
    public class DataHelper
    {
        public List<IntPair> pairs;

        public T Get<T>(List<T> list, uint primary) where T : InformationBase
        {
            int start = 0, end = list.Count;

            int count = pairs.Count;

            for (int i = 1; i < count; i++)
            {
                if (pairs[i - 1].x <= primary && pairs[i].x > primary)
                {
                    start = pairs[i - 1].y;
                    end = pairs[i].y;
                    break;
                }
            }
            for (int i = start; i < end; i++)
            {
                if (list[i].primary == primary)
                {
                    return list[i];
                }
            }
            return default;
        }

        public void Divide<T>(List<T> list) where T : InformationBase
        {
            pairs.Clear();

            list.Sort((x, y) =>
            {
                return x.primary.CompareTo(y.primary);
            });
            int step = 0, interval = 100;

            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                if (--step < 0)
                {
                    pairs.Add(new IntPair()
                    {
                        x = (int)list[i].primary,
                        y = i
                    });
                    step = interval;
                }
            }
            pairs.Add(new IntPair()
            {
                x = int.MaxValue,
                y = count
            });
        }
    }
}