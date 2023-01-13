using System.Collections;
using System.Collections.Generic;
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

        }

        public virtual void OnRecycle()
        {

        }

        public virtual void OnDestory()
        {
            GameObject.Destroy(gameObject);
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