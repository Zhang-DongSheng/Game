using UnityEngine;

namespace Game.Pool
{
    public class PoolObject : ItemBase
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

        protected virtual void OnDisable()
        {
            if (recycle)
            {
                PoolManager.Instance.Push(key, this);
            }
        }
    }
}