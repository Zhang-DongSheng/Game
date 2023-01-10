using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class PoolObject : MonoBehaviour
    {
        public GameObject GameObject => gameObject;

        public virtual void OnCreate()
        {

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
    }
}
