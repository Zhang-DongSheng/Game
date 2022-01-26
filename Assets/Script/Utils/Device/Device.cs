using System.Collections;
using UnityEngine;

namespace Device
{
    public abstract class Device<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected bool apply = true;

        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        GameObject compon = new GameObject(typeof(T).Name);
                        _instance = compon.AddComponent<T>();
                    }
                    _instance.GetComponent<Device<T>>().Initialized();
                }
                return _instance;
            }
        }
        protected abstract void Initialized();

        public abstract void Begin();

        public abstract void Stop();

        public abstract void Save();

        protected abstract IEnumerator Permissions();

        protected virtual void ApplyPermissions()
        {
            StartCoroutine(Permissions());
        }
    }
}