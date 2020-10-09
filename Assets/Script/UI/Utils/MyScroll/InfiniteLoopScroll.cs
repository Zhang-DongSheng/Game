using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// 无限滚动循环列表 自动归位 前进，后退
    /// </summary>
    public class InfiniteLoopScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        enum Direction
        {
            None,
            Horizontal,
            Vertical,
        }

        [SerializeField] private Direction direction;

        [SerializeField] private InfiniteLoopContent content;

        [SerializeField] private GameObject prefab;

        [SerializeField] private Vector2 center;

        [SerializeField] private float spring = 30f;

        [SerializeField] private float ratio = 1;

        [SerializeField, Range(3, 20)] private int count = 5;

        private Vector2 space;

        private Vector2 cell;

        private Vector2 position;

        private Vector2 vector;

        private Vector2 delta;

        private float offset;

        private Action action;

        private int current, front, back, index;

        #region Align
        private Vector2 alignPosition;

        private Vector2 alignNext;

        private Vector2 alignVector;

        private float alignRatio = 1f;

        private float alignStep;
        #endregion

        private DragStatus status;

        private readonly List<object> source = new List<object> { 1, 3, 5, 7, 9, 12.55, 15 };

        private readonly List<InfiniteLoopItem> items = new List<InfiniteLoopItem>();

        public Action<object> onValueChanged;

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

                        alignVector =  alignNext - alignPosition;

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
            OnBeginDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDrag(eventData.delta);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDrag();
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
            if (content == null) return;

            content.Clear(); items.Clear();

            for (int i = 0; i < count; i++)
            {
                items.Add(GameObject.Instantiate(prefab, content.transform).GetComponent<InfiniteLoopItem>());
            }
            content.Format();

            if (count > 1)
            {
                cell = content.Cell;

                back = 2;

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
            Format();
        }

        private void Format()
        {
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

                            index = index < 0 ? source.Count + index : index;

                            items[i].Refresh(source[index]);
                        }
                        else
                        {
                            index = i;

                            index %= source.Count;

                            items[i].Refresh(source[index]);
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
                        current = current > source.Count - 1 ? 0 : current;
                        index = current + front;
                        index %= source.Count;
                        items[i].Refresh(source[index]);
                        break;
                    case Action.Back:
                        current--;
                        current = current < 0 ? source.Count - 1 : current;
                        index = current - back + 1;
                        index = index < 0 ? source.Count + index : index;
                        items[i].Refresh(source[index]);
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
                    inside = Math.Abs(position.x - center.x) < cell.x * 0.5f;
                    break;
                case Direction.Vertical:
                    inside = Math.Abs(position.y - center.y) < cell.y * 0.5f;
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
            object result = null;

            for (int i = 0; i < items.Count; i++)
            {
                if (Between(items[i].Position, center))
                {
                    result = items[i].Source;
                    break;
                }
            }

            if (result != null)
            {
                onValueChanged?.Invoke(result);
            }
            else
            {
                Debug.LogError("xxx");
            }
        }
        #endregion

        #region Function
        public void Refresh(List<object> source)
        {
            this.source.Clear();

            this.source.AddRange(source);

            Initialize();
        }

        public void Front()
        {
            alignPosition -= cell;

            alignStep = 0;

            status = DragStatus.Align;

            Finish(center + alignPosition);
        }

        public void Back()
        {
            alignPosition += cell;

            alignStep = 0;

            status = DragStatus.Align;

            Finish(center + alignPosition);
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