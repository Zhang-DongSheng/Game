namespace UnityEngine.UI
{
    /// <summary>
    /// 无限滚动循环列表-格子基类
    /// </summary>
    public abstract class InfiniteLoopItem : MonoBehaviour
    {
        protected RectTransform target;

        protected virtual void Refresh() { }

        public void Init()
        {
            target = transform.GetComponent<RectTransform>();
        }

        public void Refresh(int index, object source)
        {
            Index = index;

            Source = source;

            Refresh();
        }

        public int Index { get; private set; }

        public object Source { get; private set; }

        public Vector2 Position
        {
            get
            {
                return target.anchoredPosition;
            }
            set
            {
                target.anchoredPosition = value;
            }
        }
    }
}