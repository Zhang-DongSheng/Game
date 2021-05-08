using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class InfiniteScrollList : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        enum Direction
        {
            None,
            Horizontal,
            Vertical,
        }

        [SerializeField] private Direction direction;

        [SerializeField] private InfiniteLayout content;

        [SerializeField] private GameObject prefab;

        [SerializeField] private ScrollRect scroll;

        [SerializeField] private Vector2 front, back;

        [SerializeField] private float distance;

        [SerializeField, Range(1, 5)] private int reserve = 2;

        [SerializeField, Range(5, 20)] private int count = 10;

        [SerializeField, Range(0.1f, 5)] private float ratio = 1;

        private Vector2 position, vector;

        private Vector2 space;

        private bool overflow;

        private bool drag;

        private int first, last;

        private DragStatus status;

        #region Align
        private Vector2 alignPosition;

        private Vector2 alighTarget;

        private Vector2 alignNext;

        private Vector2 alignVector;

        private float alignRatio = 1f;

        private float alignStep;
        #endregion

        private readonly IList source = new List<object>() { 1, 2, 3 };

        private readonly List<InfiniteItem> items = new List<InfiniteItem>();

        private void Update()
        {
            if (status == DragStatus.Align)
            {
                alignStep += Time.deltaTime * alignRatio;

                alignNext = Vector2.Lerp(alignPosition, alighTarget, alignStep);

                alignVector = alignNext - alignPosition;

                alignPosition = alignNext;

                Shift(alignVector, true);

                if (alignStep > 1)
                {
                    status = DragStatus.Idle;
                }
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
                    alignPosition = items[0].Position;

                    alighTarget = front;

                    alignStep = 0;

                    status = DragStatus.Align;
                }
                else
                {
                    status = DragStatus.Idle;
                }
            }
            else if (OverflowFront(new Vector2(1, -1)))
            {
                alignPosition = items[0].Position;

                alighTarget = front;

                alignStep = 0;

                status = DragStatus.Align;
            }
            else if (OverflowBack(new Vector2(-1, 1)))
            {
                alignPosition = items[last].Position;

                alighTarget = back;

                alignStep = 0;

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

            items.Clear(); content.Clear();

            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    InfiniteItem item = GameObject.Instantiate(prefab, content.transform).GetComponent<InfiniteItem>();
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
                                if (items[0].Index > 0)
                                {
                                    ToBack(items[0].Index - 1);
                                }
                            }
                        }
                        break;
                }
            }
        }

        private void ToFront(int index)
        {
            InfiniteItem item = items[first];

            items.RemoveAt(first);

            items.Add(item);

            items[last].Refresh(index, source[index]);

            items[last].Position = Next(items[last - 1], true);

            UpdateSpace();
        }

        private void ToBack(int index)
        {
            InfiniteItem item = items[last];

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
            if (items[first].Index == 0)
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
            if (items[last].Index == source.Count - 1)
            {
                switch (direction)
                {
                    case Direction.Horizontal:
                        return vector.x < 0 && items[last].Position.x + distance < back.x;
                    case Direction.Vertical:
                        return vector.y > 0 && items[last].Position.y - distance > back.y;
                }
            }
            return false;
        }

        private Vector2 Next(InfiniteItem item, bool forward)
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