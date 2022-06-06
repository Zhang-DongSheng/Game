using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// 不规则循环列表
    /// </summary>
    public class UnregularScrollList : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {


















        enum Direction
        {
            None,
            Horizontal,
            Vertical,
        }

        [SerializeField] private Direction direction;

        [SerializeField] private UnregularScrollLayout layout;

        [SerializeField] private ScrollRect scroll;

        [SerializeField, Range(0.1f, 5)] private float ratio = 1;

        private Vector2 position, vector;

        private Vector2 space, view;

        private Vector2 point;

        private bool drag;

        private int index, count;

        private DragStatus status;

        private readonly int[] segment = new int[4] { 0, 0, 0, 0 };

        private readonly IList source = new List<object>() { 1, 2, 3 };

        private readonly List<UnregularScrollItem> items = new List<UnregularScrollItem>();

        private readonly List<UnregularLoopInformation> cells = new List<UnregularLoopInformation>();

        public void OnBeginDrag(PointerEventData eventData)
        {
            drag = true;

            if (scroll != null)
            {
                switch (direction)
                {
                    case Direction.Horizontal:
                        drag = Utility._Vector.Horizontal(eventData.delta);
                        break;
                    case Direction.Vertical:
                        drag = Utility._Vector.Vertical(eventData.delta);
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
            //if (!overflow)
            //{
            //    if (items.Count > 0)
            //    {
            //        align.StartUp(items[0].Position, front);

            //        status = DragStatus.Align;
            //    }
            //    else
            //    {
            //        status = DragStatus.Idle;
            //    }
            //}
            //else if (OverflowFront(new Vector2(1, -1)))
            //{
            //    align.StartUp(items[0].Position, front);

            //    status = DragStatus.Align;
            //}
            //else if (OverflowBack(new Vector2(-1, 1)))
            //{
            //    align.StartUp(items[last].Position - new Vector2(0, items[last].Size.y), back);

            //    status = DragStatus.Align;
            //}
            //else
            //{
            //    status = DragStatus.Idle;
            //}
        }

        private void Initialize()
        {
            view = GetComponent<RectTransform>().rect.size;

            layout.Initialize();

            count = source.Count;

            position = Vector2.zero;

            cells.Clear();

            for (int i = 0; i < count; i++)
            {
                cells.Add(new UnregularLoopInformation()
                {
                    index = i,
                    position = position,
                    size = new Vector2(100, Random.Range(80, 150)),
                });

                switch (direction)
                {
                    case Direction.Horizontal:
                        position.x += cells[i].size.x + layout.space.x;
                        break;
                    case Direction.Vertical:
                        position.y -= cells[i].size.y + layout.space.y;
                        break;
                    default:
                        position += cells[i].size + layout.space;
                        break;
                }
            }

            switch (direction)
            {
                case Direction.Horizontal:
                    space.x = cells[count - 1].position.x + cells[count - 1].size.x;
                    layout.SetHorizontalSize(space.x);
                    break;
                case Direction.Vertical:
                    space.y = cells[count - 1].position.y - cells[count - 1].size.y;
                    space.y *= -1f;
                    layout.SetVerticalSize(space.y);
                    space.y -= view.y;
                    break;
                default:
                    space = cells[count - 1].position + cells[count - 1].size;
                    break;
            }

            position = Vector2.zero;

            layout.SetPosition(position);

            UpdateCells();
        }



        private void Shift(Vector2 delta, bool align = false)
        {
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
            position += delta;
            position.x = Mathf.Clamp(position.x, 0, space.x);
            position.y = Mathf.Clamp(position.y, 0, space.y);

            layout.SetPosition(position);

            UpdateCells();
        }

        private void UpdateCells()
        {
            count = cells.Count;

            segment[0] = count; segment[1] = 0;

            point = position * -1f;

            for (int i = 0; i < count; i++)
            {
                if (cells[i].Exist(point, view))
                {
                    if (segment[0] > i) segment[0] = i;
                    if (segment[1] < i) segment[1] = i;
                }
            }

            segment[1] = Mathf.Min(segment[1] + 1, count);

            count = items.Count;

            segment[2] = segment[3] = -1;

            for (int i = 0; i < count; i++)
            {
                if (items[i].Index >= segment[0] &&
                    items[i].Index <= segment[1])
                {
                    segment[2] = i;
                    segment[3] = items[i].Index;
                    break;
                }
            }

            index = 0;

            if (segment[2] != -1)
            {
                for (int i = 0; i < segment[2]; i++)
                {
                    items.Reverse(0, 1);
                }
                for (int i = segment[3]; i < segment[1]; i++)
                {
                    UpdateItem(index++, cells[i]);
                }
                for (int i = segment[0]; i < segment[3]; i++)
                {
                    UpdateItem(index++, cells[i]);
                }
            }
            else
            {
                for (int i = segment[0]; i < segment[1]; i++)
                {
                    UpdateItem(index++, cells[i]);
                }
            }
            for (int i = index; i < items.Count; i++)
            {
                items[i].SetActive(false);
            }
        }

        private void UpdateItem(int index, UnregularLoopInformation cell)
        {
            if (index >= items.Count)
            {
                items.Add(layout.Create());
            }
            if (items[index].Index != cell.index)
            {
                items[index].SetPosition(cell.position);
                items[index].SetSize(cell.size);
                items[index].Refresh(cell.index, source[cell.index]);
            }
            else
            {
                items[index].SetActive(true);
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
        #endregion

        enum DragStatus
        {
            Idle,
            Drag,
            Align,
            Break,
        }

        class UnregularLoopInformation
        {
            public int index;

            public Vector2 size;

            public Vector2 position;

            public bool Exist(Vector2 position, Vector2 space)
            {
                return this.position.x >= position.x - space.x - size.x &&
                       this.position.x <= position.x + space.x + size.x &&
                       this.position.y >= position.y - space.y - size.y &&
                       this.position.y <= position.y + space.y + size.y;
            }
        }
    }
}