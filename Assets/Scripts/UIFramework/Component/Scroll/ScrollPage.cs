using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollPage : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        private ScrollRect scroll;

        private float space, capacity;

        private float progress;

        private int index;

        private bool draging;

        private Status status;

        private Vector2 position;

        private Vector2 destination;

        public UnityEvent<int> onValueChanged;

        private void Awake()
        {
            scroll = GetComponent<ScrollRect>();

            scroll.horizontal = true;
        }

        private void Update()
        {
            if (draging) return;

            switch (status)
            {
                case Status.Move:
                    {
                        progress += Time.deltaTime * scroll.elasticity;

                        if (progress > 1)
                        {
                            status = Status.Complete;
                        }
                        position.x = Mathf.Lerp(position.x, destination.x, progress);

                        scroll.content.anchoredPosition = position;
                    }
                    break;
                case Status.Complete:
                    status = Status.Idle;
                    break;
            }
        }

        private IEnumerator FixedPosition(int index)
        {
            yield return new WaitForEndOfFrame();

            destination = Vector2.zero;

            space = scroll.viewport.rect.width;

            capacity = scroll.content.rect.width;

            if (capacity < space)
            {
                // 内容小于空间大小，就在起始位置
            }
            else if (index == 0)
            {
                // 第一个奖励就在起点不动了
            }
            else if (index > 0)
            {
                if (scroll.content.childCount > index &&
                    scroll.content.GetChild(index).TryGetComponent(out RectTransform target))
                {
                    destination.x = target.anchoredPosition.x - space * .5f;
                }
                destination.x *= -1f;
                // 限制在指定范围内
                if (destination.x + capacity < space)
                {
                    destination.x = space - capacity;
                }
                destination.x = Mathf.Min(0, destination.x);
            }
            position = scroll.content.anchoredPosition;

            progress = 0;

            status = Status.Move;
        }
        [ContextMenu("上一个")]
        public void Prev()
        {
            if (index > 0)
            {
                index--;
            }
            Direction(index);
        }
        [ContextMenu("下一个")]
        public void Next()
        {
            if (index < scroll.content.childCount - 1)
            {
                index++;
            }
            Direction(index);
        }

        public void Direction(int index)
        {
            int count = scroll.content.childCount;

            if (index >= count) return;

            StartCoroutine(FixedPosition(index));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            draging = true;

            status = Status.Idle;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            draging = false;

            float center = scroll.viewport.rect.width * 0.5f;

            float min = center, current;

            for (int i = 0; i < scroll.content.childCount; i++)
            {
                if (scroll.content.GetChild(i).TryGetComponent(out RectTransform target))
                {
                    current = Mathf.Abs(center - scroll.content.anchoredPosition.x - target.anchoredPosition.x);

                    if (min > current)
                    {
                        min = current; index = i;
                    }
                }
            }
            Direction(index);
        }

        enum Status
        { 
            Idle,
            Move,
            Complete,
        }
    }
}