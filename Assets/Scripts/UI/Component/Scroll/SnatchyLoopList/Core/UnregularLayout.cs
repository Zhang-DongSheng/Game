using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class UnregularLayout : MonoBehaviour
    {
        public GameObject prefab;

        public RectOffset padding;

        public Vector2 space;

        public TextAlignment alignment;

        public bool expand;

        protected float mark;

        protected Rect rect;

        protected IList list;

        protected Vector2 view;

        protected Vector2 point;

        protected Vector2 position;

        protected ScrollRect scroll;

        protected RectTransform content;

        protected Func<int, Vector2> method;

        protected readonly List<UnregularCell> cells = new List<UnregularCell>();

        protected readonly List<UnregularCell> cache = new List<UnregularCell>();

        protected readonly List<UnregularItem> items = new List<UnregularItem>();

        private void Awake()
        {
            scroll = GetComponentInParent<ScrollRect>();

            content = GetComponent<RectTransform>();

            content.anchorMin = new Vector2(0, 1);

            content.anchorMax = new Vector2(0, 1);

            content.pivot = new Vector2(0, 1);

            view = scroll.viewport.rect.size;

            view.x -= padding.left + padding.right;

            view.y -= padding.top + padding.bottom;

            scroll.onValueChanged.AddListener(OnValueChanged);
        }

        public void Initialise(IList list)
        {
            Vector2 size = prefab.GetComponent<RectTransform>().rect.size;

            Initialise(list, (int index) =>
            {
                return size;
            });
        }

        public void Initialise(IList list, Func<int, Vector2> method)
        {
            this.list = list;

            this.method = method;

            Initialise(list.Count, method);

            Variable(true);
        }

        public void Refresh(IList list)
        {
            if (this.list.Count != list.Count)
            {
                Initialise(list.Count, method);
            }
            this.list = list;

            Variable(true);
        }

        protected abstract void Initialise(int count, Func<int, Vector2> method);

        protected abstract void Variable(bool force);

        protected virtual void Calculate(Vector2 position)
        {
            cache.Clear();

            if (cells.Count == 0) return;

            int index, center = 0;

            Vector2 vector, min = position - cells[0].position;

            int count = cells.Count;

            for (int i = 0; i < count; i++)
            {
                vector = position - cells[i].position;

                if (Mathf.Pow(vector.x, 2) + Mathf.Pow(vector.y, 2) < Mathf.Pow(min.x, 2) + Mathf.Pow(min.y, 2))
                {
                    center = i; min = vector;
                }
            }

            bool up = true, down = true;

            for (int i = 0; i < count; i++)
            {
                index = center + i;

                if (center + i < count && InSide(rect, cells[index]))
                {
                    cache.Add(cells[index]);
                }
                else
                {
                    up = false;
                }
                index = center - i;

                if (index > -1 && InSide(rect, cells[index]))
                {
                    cache.Add(cells[index]);
                }
                else
                {
                    down = false;
                }

                if (!up && !down)
                {
                    break;
                }
            }
        }

        protected virtual void Detection()
        {
            if (items.Count < cache.Count)
            {
                for (int i = items.Count; i < cache.Count; i++)
                {
                    var item = Instantiate(prefab, transform).GetComponent<UnregularItem>();

                    item.Init();

                    items.Add(item);
                }
            }
        }

        protected virtual void Renovate(bool force)
        {
            int count = Math.Min(cache.Count, items.Count);

            for (int i = 0; i < count; i++)
            {
                int index = cache.FindIndex(x => x.index == items[i].Index);

                if (index > 0)
                {
                    if (force)
                    {
                        items[i].Refresh(cache[index].index, list[cache[index].index]);
                    }
                    cache.RemoveAt(index);
                }
                else
                {
                    items[i].Refresh(cache[0].index, list[cache[0].index]);

                    items[i].SetPosition(cache[0].position);

                    items[i].SetSize(cache[0].size);

                    cache.RemoveAt(0);
                }
            }
            for (int i = count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }

        protected virtual bool InSide(Rect rect, UnregularCell cell)
        {
            return rect.Contains(cell.position);
        }

        protected virtual bool Mark(Vector2 position)
        {
            return true;
        }

        protected void OnValueChanged(Vector2 value)
        {
            if (Mark(content.anchoredPosition))
            {
                Variable(false);
            }
        }
    }
}