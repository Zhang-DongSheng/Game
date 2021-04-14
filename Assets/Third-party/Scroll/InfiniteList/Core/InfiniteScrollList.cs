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

        [SerializeField] private Vector2 center;

        [SerializeField, Range(1, 5)] private int reserve = 2;

        [SerializeField, Range(5, 20)] private int count = 10;

        [SerializeField, Range(0.1f, 5)] private float ratio = 1;

        private Vector2 position, vector;

        private Vector2 space;

        private Vector2 delta;

        private bool drag;

        private int last;

        private readonly IList source = new List<object>() { 1, 2, 3 };

        private readonly List<InfiniteItem> items = new List<InfiniteItem>();

        private void Awake()
        {

        }

        private void Update()
        {

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

        }

        private void OnDrag(Vector2 delta)
        {
            this.delta += delta * ratio;

            Shift(delta * ratio);
        }

        private void OnEndDrag()
        {

        }

        private void Initialize()
        {
            int count = Mathf.Min(this.count, source.Count);

            last = count - 1;

            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    InfiniteItem item = GameObject.Instantiate(prefab, content.transform).GetComponent<InfiniteItem>();
                    item.Init();
                    items.Add(item);
                }
                items[i].Refresh(i, source[i]);

                if (i == 0)
                {
                    items[i].Position = Vector2.zero;
                }
                else
                {
                    items[i].Position = Position(items[i - 1], true);
                }
            }
            UpdateSpace();
        }

        private void Shift(Vector2 delta)
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

        private int Current
        {
            get 
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].Exist(new Vector2(0,space.x)))
                    {
                        return items[i].Index;
                    }
                }
                return items[0].Index;
            }
        }

        private Vector2 Position(InfiniteItem item, bool forward)
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

        private void ToFront(int index)
        {
            InfiniteItem item = items[0];

            items.RemoveAt(0);

            items.Add(item);

            items[last].Refresh(index, source[index]);

            items[last].Position = Position(items[last - 1], true);

            //Debug.LogError("Front");

            UpdateSpace();
        }

        private void ToBack(int index)
        {
            InfiniteItem item = items[last];

            items.RemoveAt(last);

            items.Insert(0, item);

            items[0].Refresh(index, source[index]);

            items[0].Position = Position(items[1], false) + new Vector2(0, items[0].Size.y);

            //Debug.LogError("Back");

            UpdateSpace();
        }

        private void UpdateSpace()
        {
            space = Vector2.zero;

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
    }
}