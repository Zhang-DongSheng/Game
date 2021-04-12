using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// 无限滚动循环列表 自动归位 前进，后退
    /// </summary>
    public class InfiniteLoopScrollList : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        enum Direction
        {
            None,
            Horizontal,
            Vertical,
        }

        [SerializeField] private Direction direction;

        [SerializeField] private InfiniteLoopLayout content;

        [SerializeField] private GameObject prefab;

        [SerializeField] private ScrollRect scroll;

        [SerializeField] private Vector2 center;

        [SerializeField] private float spring = 30f;

        [SerializeField] private float ratio = 1;

        [SerializeField, Range(2, 5)] private int reserve = 2;

        [SerializeField, Range(3, 20)] private int count = 5;

        private Vector2 space;

        private Vector2 cell;

        private Vector2 position;

        private Vector2 vector;

        private Vector2 delta;

        private float offset;

        private Action action;

        private int current, front, back, index;

        private bool drag;

        #region Align
        private Vector2 alignPosition;

        private Vector2 alignNext;

        private Vector2 alignVector;

        private float alignRatio = 1f;

        private float alignStep;
        #endregion

        private DragStatus status;

        private readonly IList source = new List<object>() { 1, 2, 3 };

        private readonly List<InfiniteLoopItem> items = new List<InfiniteLoopItem>();

        public Action<int, object> onValueChanged;

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            switch (status)
            {
                case DragStatus.Align:
                    {
                        alignStep += Time.deltaTime * alignRatio;

                        alignNext = Vector2.Lerp(alignPosition, center, alignStep);

                        alignVector = alignNext - alignPosition;

                        alignPosition = alignNext;

                        Shift(alignVector);

                        if (alignStep > 1)
                        {
                            status = DragStatus.Idle;
                        }
                    }
                    break;
                default: break;
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
            this.delta = Vector2.zero;

            status = DragStatus.Drag;
        }

        private void OnDrag(Vector2 delta)
        {
            this.delta += delta * ratio;

            Shift(delta * ratio);
        }

        private void OnEndDrag()
        {
            Finish();
        }

        private void Initialize()
        {
            if (source.Count == 0) return;

            if (content == null) return;

            content.Clear(); items.Clear();

            for (int i = 0; i < count; i++)
            {
                items.Add(GameObject.Instantiate(prefab, content.transform).GetComponent<InfiniteLoopItem>());
            }
            content.Format();

            current = 0;

            if (count > 1)
            {
                cell = content.Cell;

                back = reserve;

                front = count - back;

                switch (direction)
                {
                    case Direction.Horizontal:
                        space.x = -back * cell.x;
                        space.y = front * cell.x;
                        break;
                    case Direction.Vertical:
                        space.x = -back * cell.y;
                        space.y = front * cell.y;
                        break;
                }
            }
            else
            {
                space = Vector2.zero;
            }

            int index;

            for (int i = 0; i < items.Count; i++)
            {
                position = items[i].Position;

                switch (direction)
                {
                    case Direction.Horizontal:
                        if (position.x > space.y)
                        {
                            offset = position.x - space.y;

                            position.x = space.x + offset;

                            items[i].Position = position;

                            index = source.Count - (items.Count - i);

                            index = Round(index, source.Count);

                            items[i].Refresh(index, source[index]);
                        }
                        else
                        {
                            index = Round(i, source.Count);

                            items[i].Refresh(index, source[index]);
                        }
                        break;
                    case Direction.Vertical:
                        break;
                    default:
                        break;
                }
            }
        }

        private void Shift(Vector2 delta)
        {
            switch (direction)
            {
                case Direction.Horizontal:
                    vector.x = delta.x;
                    vector.y = 0;
                    break;
                case Direction.Vertical:
                    vector.x = 0;
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
                action = Action.None;

                switch (direction)
                {
                    case Direction.Horizontal:
                        if (position.x > space.y)
                        {
                            offset = position.x - space.y;
                            position.x = space.x + offset;
                            action = Action.Back;
                        }
                        else if (position.x < space.x)
                        {
                            offset = position.x - space.x;
                            position.x = space.y + offset;
                            action = Action.Front;
                        }
                        break;
                    case Direction.Vertical:
                        if (position.y > space.y)
                        {
                            offset = position.y - space.y;
                            position.y = space.x + offset;
                            action = Action.Front;
                        }
                        else if (position.y < space.x)
                        {
                            offset = position.y - space.x;
                            position.y = space.y + offset;
                            action = Action.Back;
                        }
                        break;
                    default:
                        break;
                }

                items[i].Position = position;

                switch (action)
                {
                    case Action.Front:
                        current++;
                        current = Round(current, source.Count);
                        index = Round(current + front, source.Count);
                        items[i].Refresh(index, source[index]);
                        break;
                    case Action.Back:
                        current--;
                        current = Round(current, source.Count);
                        index = Round(current - back + 1, source.Count);
                        items[i].Refresh(index, source[index]);
                        break;
                }
            }
        }

        private bool Between(Vector2 position, Vector2 center)
        {
            bool inside = false;

            switch (direction)
            {
                case Direction.Horizontal:
                    inside = Math.Abs(position.x - center.x) <= cell.x * 0.5f;
                    break;
                case Direction.Vertical:
                    inside = Math.Abs(position.y - center.y) <= cell.y * 0.5f;
                    break;
            }

            return inside;
        }

        private void Finish()
        {
            bool spring = false;

            switch (direction)
            {
                case Direction.Horizontal:
                    spring = Math.Abs(delta.x) < cell.x / 2 && Math.Abs(delta.x) > this.spring;
                    break;
                case Direction.Vertical:
                    spring = Math.Abs(delta.y) < cell.y / 2 && Math.Abs(delta.y) > this.spring;
                    break;
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (Between(items[i].Position, center))
                {
                    alignPosition = items[i].Position;
                    break;
                }
            }

            if (spring)
            {
                switch (direction)
                {
                    case Direction.Horizontal:
                        if (delta.x > 0)
                            Front();
                        else
                            Back();
                        break;
                    case Direction.Vertical:
                        if (delta.y > 0)
                            Back();
                        else
                            Front();
                        break;
                }
            }
            else
            {
                alignStep = 0;

                status = DragStatus.Align;

                Finish(center);
            }
        }

        private void Finish(Vector2 center)
        {
            bool empty = true;

            for (int i = 0; i < items.Count; i++)
            {
                if (Between(items[i].Position, center))
                {
                    onValueChanged?.Invoke(items[i].Index, items[i].Source);
                    empty = false;
                    break;
                }
            }

            if (empty)
            {
                Debug.LogError("The Item is Null!");
            }
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

        public void Front()
        {
            if (status == DragStatus.Align && alignStep < 0.1f) return;

            alignPosition -= cell;

            alignStep = 0;

            status = DragStatus.Align;

            Finish(center + alignPosition);
        }

        public void Back()
        {
            if (status == DragStatus.Align && alignStep < 0.1f) return;

            alignPosition += cell;

            alignStep = 0;

            status = DragStatus.Align;

            Finish(center + alignPosition);
        }

        public bool Lock { get; set; }
        #endregion

        #region Utils
        public static int Round(int number, int max, int min = 0)
        {
            if (number < min && max > min)
            {
                while (number < min)
                {
                    number += max - min;
                }
            }
            else if (number >= max && max != 0)
            {
                number %= max;
            }
            return number;
        }
        #endregion

        enum Action
        {
            None,
            Front,
            Back,
        }

        enum DragStatus
        {
            Idle,
            Drag,
            Align,
            Break,
        }
    }
}