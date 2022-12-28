using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UnregularScrollLayout : MonoBehaviour
    {
        public GameObject prefab;

        public RectOffset padding;

        public Vector2 space;

        private Rect rect;

        private Vector2 position;

        private ScrollRect scroll;

        private RectTransform content;

        private readonly List<UnregularScrollCell> cells = new List<UnregularScrollCell>();

        private readonly List<UnregularScrollCell> datas = new List<UnregularScrollCell>();

        private readonly List<UnregularScrollItem> items = new List<UnregularScrollItem>();

        private void Awake()
        {
            scroll = GetComponentInParent<ScrollRect>();

            content = GetComponent<RectTransform>();

            content.anchorMin = new Vector2(0, 1);

            content.anchorMax = new Vector2(0, 1);

            content.pivot = new Vector2(0, 1);

            scroll.onValueChanged.AddListener(OnValueChanged);
        }

        public void Refresh(IList list)
        {
            cells.Clear();

            position = Vector2.zero;

            position.x += padding.left;

            Vector2 size = prefab.GetComponent<RectTransform>().rect.size;

            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                cells.Add(new UnregularScrollCell()
                {
                    position = position,
                    size = size
                });

                position.x += size.x;

                position.x += space.x;
            }
            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, position.x);

            OnValueChanged(Vector2.zero);
        }

        public void Refresh(IList list, Func<int, Vector2> size)
        {
            cells.Clear();

            position = Vector2.zero;

            position.x += padding.left;

            int count = list.Count;

            for (int i = 0; i < count; i++)
            {
                cells.Add(new UnregularScrollCell()
                { 
                    position = position,
                    size = size(i)
                });

                position.x += size(i).x;

                position.x += space.x;
            }
            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, position.x);

            OnValueChanged(Vector2.zero);
        }

        private void OnValueChanged(Vector2 value)
        {
            position = -content.anchoredPosition;

            rect.position = position;

            rect.size = scroll.viewport.rect.size;

            position.x += scroll.viewport.rect.size.x * 0.5f;

            CCells(position);

            CPare();

            CItmes();
        }

        private void CCells(Vector2 position)
        {
            datas.Clear();

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
            Debug.LogError("当前中心点" + center);

            bool up = true, down = true;

            for (int i = 0; i < count; i++)
            {
                index = center + i;

                if (center + i < count && rect.Contains(cells[index].position))
                {
                    datas.Add(cells[index]);
                }
                else
                {
                    up = false;
                }
                index = center - i;

                if (index > -1 && rect.Contains(cells[index].position))
                {
                    datas.Add(cells[index]);
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

        private void CPare()
        {
            // 检测实体组件数量>=数据数量

            Debug.LogError(datas.Count);

            if (items.Count >= datas.Count)
            {

            }
            else
            {
                int count = datas.Count;

                for (int i = 0; i < count; i++)
                {
                    if (i >= items.Count)
                    {
                        var item = Instantiate(prefab, transform).GetComponent<UnregularScrollItem>();

                        item.Init();

                        items.Add(item);
                    }
                }
            }
        }

        private void CItmes()
        {
            // 刷新 List<UnregularScrollItem>。。。

            int count = Math.Min(datas.Count, items.Count);

            for (int i = 0; i < count; i++)
            {
                int index = cells.FindIndex(x => x.index == items[i].Index);

                if (index > 0)
                {
                    datas.RemoveAt(index);
                }
                else
                {
                    items[i].Refresh(datas[0].index, null);

                    items[i].SetPosition(datas[0].position);

                    items[i].SetSize(datas[0].size);

                    datas.RemoveAt(0);
                }
            }
            for (int i = count; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }

        struct UnregularScrollCell
        {
            public int index;

            public Vector2 position;

            public Vector2 size;
        }
    }
}