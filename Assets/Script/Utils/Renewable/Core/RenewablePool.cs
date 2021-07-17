using Game;
using System;
using System.Collections.Generic;

namespace UnityEngine.Renewable
{
    public class RenewablePool : Singleton<RenewablePool>
    {
        private readonly Dictionary<RPKey, RPCache> m_cache = new Dictionary<RPKey, RPCache>();

        public void Push(RPKey cache, string key, string secret, bool recent, Object value)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (m_cache.ContainsKey(cache))
            {
                m_cache[cache].Push(key, secret, recent, value);
            }
            else
            {
                m_cache.Add(cache, new RPCache()
                {
                    capacity = Capacity(cache),
                });
                m_cache[cache].Push(key, secret, recent, value);
            }
        }

        public T Pop<T>(RPKey cache, string key) where T : Object
        {
            if (string.IsNullOrEmpty(key)) return null;

            T result = null;

            if (m_cache.ContainsKey(cache))
            {
                result = m_cache[cache].Pop<T>(key);
            }

            return result;
        }

        public bool Exist(RPKey cache, string key, string secret)
        {
            if (string.IsNullOrEmpty(key)) return false;

            return m_cache.ContainsKey(cache) && m_cache[cache].Exist(key, secret);
        }

        public bool Recent(RPKey cache, string key)
        {
            if (string.IsNullOrEmpty(key)) return false;

            return m_cache.ContainsKey(cache) && m_cache[cache].Recent(key);
        }

        public void Release()
        {
            foreach (var cache in m_cache)
            {
                m_cache[cache.Key].Release();
            }
        }

        public void Release(RPKey cache, List<string> ignore, bool single = false)
        {
            foreach (var key in m_cache.Keys)
            {
                if (key == cache)
                {
                    if (ignore != null && ignore.Count > 0)
                    {
                        m_cache[key].Release(ignore);
                    }
                    else
                    {
                        m_cache[key].Release();
                    }
                }
                else
                {
                    if (!single)
                    {
                        m_cache[key].Release();
                    }
                }
            }
        }

        private int Capacity(RPKey cache)
        {
            switch (cache)
            {
                case RPKey.None:
                    return -1;
                case RPKey.ImageCover:
                    return 3;
                case RPKey.ImageComment:
                    return 3;
                case RPKey.AudioCover:
                    return 3;
                case RPKey.AssetBundleNormal:
                    return -1;
                default:
                    return -1;
            }
        }
    }

    public class RPCache
    {
        private readonly Dictionary<string, RPValue> m_cache = new Dictionary<string, RPValue>();

        public int capacity;

        public void Push(string key, string secret, bool recnet, Object value)
        {
            if (m_cache.ContainsKey(key))
            {
                m_cache[key].Replace(secret, recnet, value);
            }
            else
            {
                if (capacity > 0 && m_cache.Count > capacity)
                {
                    string abandon = AbandonKey;

                    if (!string.IsNullOrEmpty(abandon))
                    {
                        m_cache[abandon].Release();
                        m_cache.Remove(abandon);
                    }
                }
                m_cache.Add(key, new RPValue(key, secret, recnet, value));
            }
        }

        public T Pop<T>(string key) where T : Object
        {
            T result = null;

            if (m_cache.ContainsKey(key))
            {
                result = m_cache[key].source as T;
            }

            return result;
        }

        public bool Exist(string key, string secret)
        {
            if (m_cache.ContainsKey(key))
            {
                return string.IsNullOrEmpty(secret) || m_cache[key].secret == secret;
            }
            return false;
        }

        public bool Recent(string key)
        {
            if (m_cache.ContainsKey(key))
            {
                return m_cache[key].recent;
            }
            return false;
        }

        public void Release(List<string> ignore = null)
        {
            if (ignore != null && ignore.Count > 0)
            {
                List<RPValue> _temp = new List<RPValue>();

                foreach (var key in m_cache.Keys)
                {
                    if (ignore.Contains(key))
                    {
                        _temp.Add(m_cache[key]);
                    }
                    else
                    {
                        m_cache[key].Release();
                    }
                }
                m_cache.Clear();

                if (_temp.Count > 0)
                {
                    for (int i = 0; i < _temp.Count; i++)
                    {
                        m_cache.Add(_temp[i].key, _temp[i]);
                    }
                    _temp.Clear();
                }
            }
            else
            {
                foreach (var key in m_cache.Keys)
                {
                    m_cache[key].Release();
                }
                m_cache.Clear();
            }
        }

        private string AbandonKey
        {
            get
            {
                string key = null;

                int index = 0;

                foreach (var cache in m_cache)
                {
                    if (index == 0)
                    {
                        key = cache.Key;
                        break;
                    }
                }

                return key;
            }
        }
    }

    public class RPValue
    {
        public string key;

        public string secret;

        public int reference;

        public bool recent;

        public Object source;

        public RPValue(string key, string secret, bool recent, Object source)
        {
            this.key = key;

            this.secret = secret;

            this.reference = 0;

            this.recent = recent;

            this.source = source;
        }

        public void Replace(string secret, bool recent, Object source)
        {
            Release();

            Debug.LogWarning("替换成功:" + key);

            this.secret = secret;

            this.recent = recent;

            this.source = source;
        }

        public void Release()
        {
            if (source != null)
            {
                try
                {
                    Destroy.Release(source);

                    Object.DestroyImmediate(source, true);
                }
                catch (Exception e)
                {
                    Debug.LogError("Release Asset Fail : " + e.Message);
                }
                //Resources.UnloadUnusedAssets();
            }
        }
    }

    public enum RPKey
    {
        None,               //常驻资源
        ImageCover,         //封面
        ImageComment,       //评论
        AudioCover,         //封面
        AssetBundleNormal,
    }
}