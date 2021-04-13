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

        [SerializeField, Range(5, 20)] private int count = 10;

        [SerializeField, Range(5, 20)] private float ratio = 1;

        private Vector2 position, vector;

        private Vector2 delta;

        private float offset;

        private bool drag;

        private Vector2 space;

        private readonly IList source;

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
            for (int i = 0; i < count; i++)
            {
                if (i >= items.Count)
                {
                    InfiniteItem item = GameObject.Instantiate(prefab, content.transform).GetComponent<InfiniteItem>();
                    items.Add(item);
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
                //action = Action.None;

                switch (direction)
                {
                    case Direction.Horizontal:
                        if (position.x > space.y)
                        {
                            offset = position.x - space.y;
                            position.x = space.x + offset;
                            //action = Action.Back;
                        }
                        else if (position.x < space.x)
                        {
                            offset = position.x - space.x;
                            position.x = space.y + offset;
                            //action = Action.Front;
                        }
                        break;
                    case Direction.Vertical:
                        if (position.y > space.y)
                        {
                            offset = position.y - space.y;
                            position.y = space.x + offset;
                            //action = Action.Front;
                        }
                        else if (position.y < space.x)
                        {
                            offset = position.y - space.x;
                            position.y = space.y + offset;
                            //action = Action.Back;
                        }
                        break;
                    default:
                        break;
                }
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