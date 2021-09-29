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
                for (int i = segment.from; i < segment.to; i++)
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

            int _from, _to;

            for (int i = 0; i < step; i++)
            {
                _from = i * interval;

                _to = Mathf.Min(i * interval + interval - 1, count - 1);

                segments.Add(new Segment()
                {
                    from = _from,
                    to = _to,
                    begin = list[_from].identification,
                    end = list[_to].identification,
                });
            }
        }

        protected virtual void Editor()
        {

        }
        [ContextMenu("Editor")]
        protected void EditorMenu()
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
        public int from, to;

        public int begin, end;

        public bool Exist(int value)
        {
            return value >= begin && value <= end;
        }
    }
}