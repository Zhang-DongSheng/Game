namespace UnityEngine.UI
{
    public abstract class InfiniteLoopItem : MonoBehaviour
    {
        public int Index { get; private set; }

        public object Source { get; private set; }

        protected virtual void Refresh() { }

        public void Refresh(int index, object source)
        {
            Index = index; Source = source;

            Refresh();
        }

        public Vector2 Position
        {
            get
            {
                return transform.localPosition;
            }
            set
            {
                transform.localPosition = value;
            }
        }
    }
}