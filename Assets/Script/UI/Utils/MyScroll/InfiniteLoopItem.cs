namespace UnityEngine.UI
{
    public abstract class InfiniteLoopItem : MonoBehaviour
    {
        protected object source;

        protected virtual void Refresh() { }

        public void Refresh(object source)
        {
            this.source = source;

            this.Refresh();
        }

        public object Source
        {
            get
            {
                return source;
            }
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