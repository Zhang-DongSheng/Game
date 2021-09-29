using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public abstract class DataBase : ScriptableObject
    {
        protected readonly List<Segment> segments = new List<Segment>();

        protected Segment segment;

        protected T QuickLook<T>(List<T> list, int identification) where T : InformationBase
        {
            if (segments.Count == 0 && list.Count > 0)
            {
                Subsection(list);
            }

            segment = segments.Find(x => x.Exist(identification));

            if (segment == null)
            {
                return list.Find(x => x.identification == identification);
            }
            else
            {
                for (int i = segment.begin; i < segment.end; i++)
                {
                    if (list[i].identification == identification)
                    {
                        return list[i];
                    }
                }
                return null;
            }
        }

        protected void Subsection<T>(List<T> list, int interval = 100) where T : InformationBase
        {
            segments.Clear();

            int count = list.Count;

            int step = count / interval;

            step = Mathf.Clamp(step, 1, 100);

            interval = Mathf.CeilToInt(count / (float)step);

            int _begin, _end;

            for (int i = 0; i < step; i++)
            {
                _begin = i * interval;

                _end = Mathf.Min(i * interval + interval - 1, count - 1);

                segments.Add(new Segment()
                {
                    begin = _begin,
                    end = _end,
                    from = list[_begin].identification,
                    to = list[_end].identification,
                });
            }
        }

        protected virtual void Editor()
        {

        }
        [ContextMenu("Editor")]
        protected void MenuEditor()
        {
            Editor();
        }
    }

    public class InformationBase
    {
        public int identification;
    }

    public class Segment
    {
        public int begin, end;

        public int from, to;

        public bool Exist(int value)
        {
            return value >= from && value <= to;
        }
    }
}