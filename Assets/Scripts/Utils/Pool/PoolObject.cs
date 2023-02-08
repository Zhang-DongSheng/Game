using UnityEngine;

namespace Game.Pool
{
    public class PoolObject : MonoBehaviour
    {
        public bool recycle;

        public string key { get; private set; }

        public virtual void OnCreate(string key)
        {
            this.key = key;
        }

        public virtual void OnFetch()
        {
            SetActive(true);
        }

        public virtual void OnRecycle()
        {
            SetActive(false);
        }

        public virtual void OnDestory()
        {
            GameObject.Destroy(gameObject);
        }

        public virtual void SetActive(bool active)
        {
            if (gameObject && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }

        protected virtual void OnDisable()
        {
            if (recycle)
            {
                PoolManager.Instance.Push(key, this);
            }
        }
    }
}