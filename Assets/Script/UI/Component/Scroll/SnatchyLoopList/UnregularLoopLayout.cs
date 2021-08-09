namespace UnityEngine.UI
{
    public class UnregularLoopLayout : MonoBehaviour
    {
        public Vector2 space = Vector2.zero;

        public void Clear()
        {
            if (transform != null && transform.childCount > 0)
            {
                for (int i = transform.childCount - 1; i > -1; i--)
                {
                    GameObject.Destroy(transform.GetChild(i).gameObject);
                }
            }
        }
    }
}