using UnityEngine;

namespace Game.Pool
{
    public class PoolObject : ItemBase
    {
        [SerializeField] private bool recycle;

        private string key;

        public virtual void OnCreate(string key)
        {
            this.key = key;
        }

        public virtual void OnFetch()
        {
            SetActive(true);
        }

        public virtual void OnRecycle(Transform parent)
        {
            try
            {
                transform.SetParent(parent);
            }
            catch
            {

            }
            SetActive(false);
        }

        public virtual void OnRemove()
        {
            if (gameObject)
            {
                GameObject.Destroy(gameObject);
            }
        }

        protected override void OnVisible(bool active)
        {
            if (active) return;

            if (recycle)
            {
                PoolManager.Instance.Push(key, this);
            }
        }
    }
}