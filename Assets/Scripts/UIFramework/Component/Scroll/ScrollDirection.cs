using System;
using System.Collections;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollDirection : MonoBehaviour
    {
        [SerializeField] private ScrollRect scroll;

        private float space, capacity;

        private Vector2 position;

        public void Direction(int index, Func<bool> condition = null)
        {
            int count = scroll.content.childCount;

            if (index >= count) return;

            StartCoroutine(FixedPosition(index, condition));
        }

        private IEnumerator FixedPosition(int index, Func<bool> condition)
        {
            if (condition != null)
            {
                yield return new WaitUntil(condition);
            }
            yield return new WaitForEndOfFrame();

            position = Vector2.zero;

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
                index--;

                if (scroll.content.childCount > index &&
                    scroll.content.GetChild(index).TryGetComponent(out RectTransform target))
                {
                    position.x = target.anchoredPosition.x * -1;
                }
                // 限制在指定范围内
                if (position.x + capacity < space)
                {
                    position.x = space - capacity;
                }
                position.x = Mathf.Min(0, position.x);
            }
            scroll.content.anchoredPosition = position;
        }
    }
}