using System;
using System.Collections.Generic;

namespace UnityEngine
{
    [RequireComponent(typeof(PoolManager))]
    public class ResourcesFactory : MonoSingleton<ResourcesFactory>
    {
        private readonly Dictionary<string, GameObject> m_prefab = new Dictionary<string, GameObject>();

        private PoolManager m_pool;

        private void Awake()
        {
            m_pool = GetComponent<PoolManager>();
        }

        public GameObject Create(string key)
        {
            GameObject result = null;

            if (!string.IsNullOrEmpty(key))
            {
                result = m_pool.Pop(key);
            }

            return result;
        }

        public GameObject Create(string key, string path, Transform parent = null)
        {
            GameObject result = null;

            if (!string.IsNullOrEmpty(key))
            {
                result = m_pool.Pop(key);

                if (result == null)
                {
                    try
                    {
                        if (m_prefab.ContainsKey(key) && m_prefab[key] != null)
                        {
                            result = Instantiate(m_prefab[key], parent);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(path))
                            {
                                path = key;
                            }
                            GameObject prefab = Resources.Load<GameObject>(path);
                            result = Instantiate(prefab, parent);
                            m_prefab.Add(key, prefab);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
                else
                {
                    if (result.transform.parent != parent)
                    {
                        result.transform.SetParent(parent);
                    }
                }
            }

            return result;
        }

        public GameObject Create(string key, GameObject prefab, Transform parent = null)
        {
            GameObject result = null;

            if (!string.IsNullOrEmpty(key))
            {
                result = m_pool.Pop(key);

                if (result == null)
                {
                    try
                    {
                        if (m_prefab.ContainsKey(key) && m_prefab[key] != null)
                        {
                            result = Instantiate(m_prefab[key], parent);
                        }
                        else
                        {
                            result = Instantiate(prefab, parent);
                            m_prefab.Add(key, prefab);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
                else
                {
                    if (result.transform.parent != parent)
                    {
                        result.transform.SetParent(parent);
                    }
                }
            }

            return result;
        }

        public void Destroy(string key, GameObject go)
        {
            m_pool.Push(key, go);
        }
    }
}