using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// 不规则循环列表
    /// </summary>
    public class UnregularLoopScrollList : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        enum Direction
        {
            None,
            Horizontal,
            Vertical,
        }

        [SerializeField] private Direction direction;

        [SerializeField] private UnregularLoopLayout content;

        [SerializeField] private GameObject prefab;

        [SerializeField] private ScrollRect scroll;

        [SerializeField] private ScrollAlign align;

        [SerializeField] private float distance;

        [SerializeField, Range(1, 5)] private int reserve = 2;

        [SerializeField, Range(5, 20)] private int count = 10;

        [SerializeField, Range(0.1f, 5)] private float ratio = 1;

        private Vector2 position, vector;

        private Vector2 front, back;

        private Vector2 space;

        private bool overflow;

        private bool drag;

        private int first, last;

        private DragStatus status;

        private readonly IList source = new List<object>() { 1, 2, 3 };

        private readonly List<UnregularLoopItem> items = new List<UnregularLoopItem>();

        protected override void Awake()
        {
            align.onValueChanged = (vector) =>
            {
                Shift(vector, true);
            };
            align.onCompleted = () =>
            {
                status = DragStatus.Idle;
            };
        }

        private void Update()
        {
            switch (status)
            {
                case DragStatus.Align:
                    {
                        align.Update();
                    }
                    break;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            drag = true;

            if (scroll != null)
            {
                switch (direction)
                {
                    case Direction.Horizontal:
                        drag = ScrollUtils.Horizontal(eventData.delta);
                        break;
                    case Direction.Vertical:
                        drag = ScrollUtils.Vertical(eventData.delta);
                        break;
                }
            }

            if (drag)
            {
                OnBeginDrag();
            }
            else
            {
                scroll.OnBeginDrag(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (drag)
            {
                OnDrag(eventData.delta);
            }
            else
            {
                scroll.OnDrag(eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (drag)
            {
                OnEndDrag();
            }
            else
            {
                scroll.OnEndDrag(eventData);
            }
        }

        #region Core
        private void OnBeginDrag()
        {
            status = DragStatus.Drag;
        }

        private void OnDrag(Vector2 delta)
        {
            Shift(delta * ratio);
        }

        private void OnEndDrag()
        {
            if (!overflow)
            {
                if (items.Count > 0)
                {
                    align.StartUp(items[0].Position, front);

                    status = DragStatus.Align;
                }
                else
                {
                    status = DragStatus.Idle;
                }
            }
            else if (OverflowFront(new Vector2(1, -1)))
            {
                align.StartUp(items[0].Position, front);

                status = DragStatus.Align;
            }
            else if (OverflowBack(new Vector2(-1, 1)))
            {
                align.StartUp(items[last].Position - new Vector2(0, items[last].Size.y), back);

                status = DragStatus.Align;
            }
            else
            {
                status = DragStatus.Idle;
            }
        }

        private void Initialize()
        {
            int count = Mathf.Min(this.count, source.Count);

            first = 0; last = count - 1;

            front = new Vector2(0, 0);

            if (TryGetComponent(out RectTransform target))
            {
                back = new Vector2(target.rect.width, target.rect.height * -1);
            }
            items.Clear(); content.Clear();

            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    UnregularLoopItem item = GameObject.Instantiate(prefab, content.transform).GetComponent<UnregularLoopItem>();
                    item.Init();
                    items.Add(item);
                }
                items[i].Refresh(i, source[i]);

                if (i == first)
                {
                    items[i].Position = front;
                }
                else
                {
                    items[i].Position = Next(items[i - 1], true);
                }
            }
            UpdateSpace(); Overflow(count - 1);
        }

        private void Shift(Vector2 delta, bool align = false)
        {
            if (!align && OverflowFront(delta, distance)) return;
            else if (!align && OverflowBack(delta, distance)) return;

            vector = Vector2.zero;

            switch (direction)
            {
                case Direction.Horizontal:
                    vector.x = delta.x;
                    break;
                case Direction.Vertical:
                    vector.y = delta.y;
                    break;
                default:
                    vector = delta;
                    break;
            }

            for (int i = 0; i < items.Count; i++)
            {
                position = items[i].Position;

                position += vector;

                items[i].Position = position;

                switch (direction)
                {
                    case Direction.Horizontal:

                        break;
                    case Direction.Vertical:
                        {
                            if (vector.y > 0 && items[i].Position.y > space.x)
                            {
                                if (items[last].Index < source.Count - 1)
                                {
                                    ToFront(items[last].Index + 1);

                                    position = items[i].Position;

                                    position += vector;

                                    items[i].Position = position;
                                }
                            }
                            else if (vector.y < 0 && items[i].Position.y < space.y)
                            {
                                if (items[first].Index > 0)
                                {
                                    ToBack(items[first].Index - 1);
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void ToFront(int index)
        {
            UnregularLoopItem item = items[first];

            items.RemoveAt(first);

            items.Add(item);

            items[last].Refresh(index, source[index]);

            items[last].Position = Next(items[last - 1], true);

            UpdateSpace();
        }

        private void ToBack(int index)
        {
            UnregularLoopItem item = items[last];

            items.RemoveAt(last);

            items.Insert(first, item);

            items[first].Refresh(index, source[index]);

            items[first].Position = Next(items[first + 1], false) + new Vector2(0, items[first].Size.y);

            UpdateSpace();
        }

        private void UpdateSpace()
        {
            space = Vector2.zero;

            if (items.Count >= reserve)
            {
                for (int i = 0; i < reserve; i++)
                {
                    space.x += content.space.y;

                    space.x += items[i].Size.y;
                }
                for (int i = reserve; i < items.Count; i++)
                {
                    space.y -= items[i].Size.y;

                    space.y -= content.space.y;
                }
            }
        }

        private void Overflow(int index)
        {
            if (items.Count > 0)
            {
                switch (direction)
                {
                    case Direction.Horizontal:
                        overflow = items[index].Position.x >= back.x;
                        break;
                    case Direction.Vertical:
                        overflow = items[index].Position.y <= back.y;
                        break;
                    default:
                        overflow = false;
                        break;
                }
            }
            else
            {
                overflow = false;
            }
        }

        private bool OverflowFront(Vector2 vector, float distance = 0)
        {
            if (items.Count > first && items[first].Index == 0)
            {
                switch (direction)
                {
                    case Direction.Horizontal:
                        return vector.x > 0 && items[first].Position.x - distance > front.x;
                    case Direction.Vertical:
                        return vector.y < 0 && items[first].Position.y + distance < front.y;
                }
            }
            return false;
        }

        private bool OverflowBack(Vector2 vector, float distance = 0)
        {
            if (items.Count > last && items[last].Index == source.Count - 1)
            {
                switch (direction)
                {
                    case Direction.Horizontal:
                        return vector.x < 0 && items[last].Position.x + distance < back.x;
                    case Direction.Vertical:
                        return vector.y > 0 && items[last].Position.y - items[last].Size.y - distance > back.y;
                }
            }
            return false;
        }

        private Vector2 Next(UnregularLoopItem item, bool forward)
        {
            Vector2 position;

            if (forward)
            {
                position = item.Position;

                position = new Vector2(Mathf.Abs(position.x), Mathf.Abs(position.y));

                position += item.Size;

                position += content.space;

                position *= -1;
            }
            else
            {
                position = item.Position;

                position += content.space;
            }

            switch (direction)
            {
                case Direction.Horizontal:
                    position.y = 0;
                    break;
                case Direction.Vertical:
                    position.x = 0;
                    break;
            }
            return position;
        }
        #endregion

        #region Function
        public void Refresh(IList source)
        {
            this.source.Clear();

            for (int i = 0; i < source.Count; i++)
            {
                this.source.Add(source[i]);
            }
            Initialize();
        }
        #endregion

        enum DragStatus
        {
            Idle,
            Drag,
            Align,
            Break,
        }
    }
}