namespace UnityEngine.UI
{
    [System.Serializable]
    public class LoopScrollPrefabSource
    {
        public string key;

        public virtual GameObject Pop()
        {
            return Factory.Instance.Pop(key) as GameObject;
        }

        public virtual void Push(Transform go)
        {
            Factory.Instance.Push(key, go.gameObject);
        }
    }
}