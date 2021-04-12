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

        private bool drag;

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
        public void Initialize()
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

        private void OnBeginDrag()
        {

        }

        private void OnDrag(Vector2 vector)
        {

        }

        private void OnEndDrag()
        {

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


        }
        #endregion
    }
}