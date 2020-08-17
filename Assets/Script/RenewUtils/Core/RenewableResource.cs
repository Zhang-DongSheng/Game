using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.Networking;

namespace UnityEngine
{
    public class RenewableResource : MonoSingleton<RenewableResource>
    {
        public const string Path_Android = "file://";

        public int DownloadLimit = 3;

        private string key;

        private string url;

        private string path;

        private float timer;

        private readonly Dictionary<string, Task> m_task = new Dictionary<string, Task>();

        private readonly Dictionary<string, Object> m_cache = new Dictionary<string, Object>();

        private RenewableResource() { }

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > 10)
            {
                if (m_task.Count > 0)
                {
                    TryAgainDownload();
                }
                timer = 0;
            }
        }

        public void Init()
        {
            //Get the latest resources list ...
            if (true)
            {
                DownloadLimit = 1; RenewableResourceUpdate.Instance.Load();
            }
        }

        public void Get(string key, string url = "", StorageClass store = StorageClass.None, DownloadFileType fileType = DownloadFileType.None, Action<byte[], Object> success = null, Action fail = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.key = key;

            this.url = string.IsNullOrEmpty(url) ? Url : url;

            this.url += this.key;

            this.path = Path + this.key;

            if (m_cache.ContainsKey(this.key))
            {
                success?.Invoke(null, m_cache[this.key]);
            }
            else
            {
                if (m_task.ContainsKey(this.key))
                {
                    m_task[this.key].AddListener(success, fail);
                }
                else
                {
                    m_task.Add(this.key, new Task(this.key, this.url, path, fileType, store)
                    {
                        local = File.Exists(this.path),
                        status = TaskStatus.Ready,
                    });
                    m_task[this.key].AddListener(success, fail);
                    Next();
                }
            }
        }

        public void TryAgainDownload()
        {
            if (m_task.Count > 0)
            {
                foreach (var task in m_task)
                {
                    if (task.Value.status == TaskStatus.Fail)
                    {
                        m_task[task.Key].status = TaskStatus.Ready;
                    }
                }
                Next();
            }
        }

        private void Next()
        {
            if (m_task.Count > 0)
            {
                int loading, count = 0;

                foreach (var task in m_task.Values)
                {
                    if (task.status == TaskStatus.Downloading)
                    {
                        count++;
                    }
                }

                loading = count;

                foreach (var task in m_task.Values)
                {
                    switch (task.status)
                    {
                        case TaskStatus.Ready:
                            if (loading++ < Count)
                            {
                                m_task[task.Key].status = TaskStatus.Downloading;

                                //Debug.LogFormat("<color=red>Downloading ... </color><color=green>[{0}]</color>:<color=blue>[{1}]</color>", task.Key, task.Path);

                                if (task.local)
                                {
                                    StartCoroutine(Load(task.Key, task.Path));
                                }
                                else
                                {
                                    StartCoroutine(Dowmload(task.Key, task.Url));
                                }
                            }
                            break;
                    }
                    if (loading >= Count) break;
                }
            }
        }

        private IEnumerator Dowmload(string key, string url)
        {
            if (m_task.ContainsKey(key))
            {
                UnityWebRequest request = Request(m_task[key].Type, url);

                yield return request.SendWebRequest();

                if (request.isDone)
                {
                    if (request.isHttpError || request.isNetworkError)
                    {
                        if (m_task.ContainsKey(key))
                        {
                            if (NetworkConnection)
                            {
                                m_task[key].fail?.Invoke(); m_task.Remove(key);
                            }
                            else
                            {
                                m_task[key].fail?.Invoke();
                                m_task[key].status = TaskStatus.Fail;
                            }
                        }
                        Debug.LogWarningFormat("<color=red>{0}</color>, Url:<color=blue>{1}</color>", request.error, url);
                    }
                    else
                    {
                        if (m_task.ContainsKey(key))
                        {
                            m_task[key].content = DownloadHandler(m_task[key].Type, request);
                            m_task[key].success?.Invoke(m_task[key].Type != DownloadFileType.Bundle ? request.downloadHandler.data : null, m_task[key].content);
                            m_task[key].status = TaskStatus.Complete;
                            switch (m_task[key].Store)
                            {
                                case StorageClass.Write:
                                    if (m_task[key].Type != DownloadFileType.Bundle)
                                        RenewableFile.Write(m_task[key].Path, request.downloadHandler.data);
                                    break;
                                case StorageClass.Cache:
                                    if (!m_cache.ContainsKey(key))
                                        m_cache.Add(key, m_task[key].content);
                                    break;
                            }
                            m_task.Remove(key);
                        }
                    }
                    Next();
                }
            }
        }

        private IEnumerator Load(string key, string path)
        {
#if UNITY_ANDROID
            if (!path.StartsWith(Path_Android)) path = Path_Android + path;
#endif
            UnityWebRequest request = Request(m_task[key].Type, path);

            yield return request.SendWebRequest();

            if (request.isDone)
            {
                if (request.isHttpError || request.isNetworkError)
                {
                    if (m_task.ContainsKey(key))
                    {
                        m_task[key].fail?.Invoke(); m_task.Remove(key);
                    }
                    Debug.LogWarningFormat("{0}, url:{1}", request.error, path);
                }
                else
                {
                    if (m_task.ContainsKey(key))
                    {
                        m_task[key].content = DownloadHandler(m_task[key].Type, request);

                        m_task[key].success?.Invoke(m_task[key].Type != DownloadFileType.Bundle ? request.downloadHandler.data : null, m_task[key].content);

                        if (RenewableResourceUpdate.Instance.Validation(m_task[key].Key, request.downloadHandler.data))
                        {
                            m_task[key].status = TaskStatus.Complete;
                            m_task.Remove(key);
                        }
                        else
                        {
                            m_task[key].local = false;
                            m_task[key].status = TaskStatus.Ready;
                        }
                    }
                }
                Next();
            }
        }

        private UnityWebRequest Request(DownloadFileType fileType, string url)
        {
            switch (fileType)
            {
                case DownloadFileType.Image:
                    return UnityWebRequestTexture.GetTexture(url);
                case DownloadFileType.Audio:
#if UNITY_EDITOR && UNITY_STANDALONE
                    return UnityWebRequest.Get(url);
#else
                    return UnityWebRequestMultimedia.GetAudioClip(url, DetectAudioType(url));
#endif
                case DownloadFileType.Bundle:
                    return UnityWebRequestAssetBundle.GetAssetBundle(url);
                default:
                    return UnityWebRequest.Get(url);
            }
        }

        private Object DownloadHandler(DownloadFileType fileType, UnityWebRequest request)
        {
            switch (fileType)
            {
                case DownloadFileType.Image:
                    return DownloadHandlerTexture.GetContent(request);
                case DownloadFileType.Audio:
#if UNITY_EDITOR && UNITY_STANDALONE
                    return null;
#else
                    return DownloadHandlerAudioClip.GetContent(request);
#endif
                case DownloadFileType.Bundle:
                    return DownloadHandlerAssetBundle.GetContent(request);
                default:
                    return null;
            }
        }

        private AudioType DetectAudioType(string url)
        {
            AudioType audioType;

            string[] param = url.Split('.');

            string suffix = param.Length > 1 ? param[param.Length - 1].ToLower() : string.Empty;

            switch (suffix)
            {
                case "mp3":
#if UNITY_ANDROID
                    audioType = AudioType.MPEG;
#elif UNITY_IOS
                    audioType =  AudioType.AUDIOQUEUE;
#else
                    audioType = AudioType.MPEG;
#endif
                    break;
                case "wav":
                    audioType = AudioType.WAV;
                    break;
                case "ogg":
                    audioType = AudioType.OGGVORBIS;
                    break;
                case "vag":
                    audioType = AudioType.VAG;
                    break;
                default:
                    audioType = AudioType.UNKNOWN;
                    break;
            }

            return audioType;
        }

        private string Url
        {
            get
            {
                return "https://branchapptest-1302051570.cos.ap-beijing.myqcloud.com/";
            }
        }

        private string Path { get { return Application.persistentDataPath + "/"; } }

        private int Count { get { return DownloadLimit; } }

        private bool NetworkConnection
        {
            get
            {
                return Application.internetReachability != NetworkReachability.NotReachable;
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        class Task : IDisposable
        {
            private readonly string _key;

            private readonly string _url;

            private readonly string _path;

            private readonly DownloadFileType _type;

            private readonly StorageClass _store;

            public TaskStatus status;

            public bool local;

            public int order;

            public Object content;

            public Action<byte[], Object> success;

            public Action fail;

            public string Key { get { return _key; } }

            public string Url { get { return _url; } }

            public string Path { get { return _path; } }

            public DownloadFileType Type { get { return _type; } }

            public StorageClass Store { get { return _store; } }

            public Task(string key, string url, string path, DownloadFileType type, StorageClass store)
            {
                this._key = key;

                this._url = url;

                this._path = path;

                this._type = type;

                this._store = store;
            }

            public void AddListener(Action<byte[], Object> success, Action fail)
            {
                if (success != null)
                {
                    if (this.success != null)
                        this.success += success;
                    else
                        this.success = success;
                }
                if (fail != null)
                {
                    if (this.fail != null)
                        this.fail += fail;
                    else
                        this.fail = fail;
                }
            }

            public void Dispose()
            {
                content = null; success = null;
            }
        }

        enum TaskStatus
        {
            Ready,
            Downloading,
            Complete,
            Error,
            Fail,
        }
    }

    public class RenewablePool : Singleton<RenewablePool>
    {
        private readonly Dictionary<CacheType, Cache> m_cache = new Dictionary<CacheType, Cache>();

        public void Push(CacheType cache, string key, Object value)
        {
            if (string.IsNullOrEmpty(key)) return;

            if( m_cache.ContainsKey(cache) )
            {
                m_cache[cache].Push(key, value);
            }
            else
            {
                m_cache.Add(cache, new Cache()
                {
                    capacity = Capacity(cache),
                });
                m_cache[cache].Push(key, value);
            }
        }

        public T Pop<T>(CacheType cache, string key) where T : Object
        {
            if (string.IsNullOrEmpty(key)) return null;

            T result = null;

            if (m_cache.ContainsKey(cache))
            {
                result = m_cache[cache].Pop<T>(key);
            }

            return result;
        }

        public bool Exist(CacheType cache, string key)
        {
            if (string.IsNullOrEmpty(key)) return false;

            return m_cache.ContainsKey(cache) && m_cache[cache].Exist(key);
        }

        private int Capacity(CacheType cache)
        {
            switch (cache)
            {
                case CacheType.None:
                    return -1;
                case CacheType.Image_Cover:
                    return 3;
                case CacheType.Image_Comment:
                    return 3;
                case CacheType.Audio_Cover:
                    return 3;
                default:
                    return -1;
            }
        }

        public void Release()
        {
            foreach (var cache in m_cache)
            {
                if (cache.Key != CacheType.None)
                {
                    m_cache[cache.Key].Release();
                }
            }
        }

        public void Release(CacheType cache, string ignore)
        {
            foreach (var key in m_cache.Keys)
            {
                if (key != CacheType.None)
                {
                    if (key != cache)
                    {
                        m_cache[key].Release();
                    }
                    else 
                    {
                        m_cache[key].Release(ignore);
                    }
                }
            }
        }

        public void ReleaseAll()
        {
            foreach (var cache in m_cache)
            {
                m_cache[cache.Key].Release();
            }
            m_cache.Clear();
        }

        class Cache
        {
            private readonly Dictionary<string, Object> m_cache = new Dictionary<string, Object>();

            public int capacity;

            public void Push(string key, Object value)
            {
                if (m_cache.ContainsKey(key))
                {
                    m_cache[key] = value;
                }
                else
                {
                    if (capacity > 0 && m_cache.Count > capacity)
                    {
                        string abandon = AbandonKey;

                        if (!string.IsNullOrEmpty(abandon))
                        {
                            Release(m_cache[abandon]);
                            m_cache.Remove(abandon);
                        }
                    }
                    m_cache.Add(key, value);
                }
            }

            public T Pop<T>(string key) where T : Object
            {
                T result = null;

                if (m_cache.ContainsKey(key))
                {
                    result = m_cache[key] as T;
                }

                return result;
            }

            public bool Exist(string key)
            {
                return m_cache.ContainsKey(key);
            }

            public void Release(string ignore = null)
            {
                if (string.IsNullOrEmpty(ignore))
                {
                    foreach (var key in m_cache.Keys)
                    {
                        Release(m_cache[key]);
                    }
                    m_cache.Clear();
                }
                else
                {
                    Object value = null;

                    foreach (var key in m_cache.Keys)
                    {
                        if (key != ignore)
                        {
                            Release(m_cache[key]);
                        }
                        else
                        {
                            value = m_cache[key];
                        }
                    }
                    m_cache.Clear();

                    if (value != null)
                    {
                        m_cache.Add(ignore, value);
                    }
                }
            }

            private void Release(Object asset)
            {
                if (asset != null)
                {
                    try
                    {
                        Object.Destroy(asset);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Release Asset Fail : " + e.Message);
                    }
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
    }

    public class RenewableResourceUpdate : Singleton<RenewableResourceUpdate>
    {
        private const string Path = "information/md5file";

        private const string Extension = ".txt";

        private readonly Dictionary<string, string> m_secret = new Dictionary<string, string>();

        public void Load(string path = "")
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Path + Extension;
            }
            RenewableResource.Instance.Get(path, success: Load);
        }

        private void Load(byte[] buffer, Object result)
        {
            string content = Encoding.Default.GetString(buffer);

            content = content.Replace("\r\n", "^");

            string[] rows = content.Split('^');

            for (int i = 0; i < rows.Length; i++)
            {
                string[] value = rows[i].Split('|');

                if (value.Length == 2 && !string.IsNullOrEmpty(value[0]))
                {
                    if (m_secret.ContainsKey(value[0]))
                    {
                        m_secret[value[0]] = value[1];
                    }
                    else
                    {
                        m_secret.Add(value[0], value[1]);
                    }
                }
            }

            RenewableResource.Instance.DownloadLimit = 3;
        }

        public bool Validation(string key, byte[] buffer)
        {
            if (string.IsNullOrEmpty(key) || !m_secret.ContainsKey(key)) return true;

            string md5 = buffer != null ? ComputeMD5(buffer) : string.Empty;

            bool equal = md5 == m_secret[key];

            if (equal)
            {
                m_secret.Remove(key);
            }

            return equal;
        }

        private string ComputeMD5(byte[] buffer)
        {
            string result = string.Empty;

            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();

                byte[] hash = md5.ComputeHash(buffer);

                foreach (byte v in hash)
                {
                    result += Convert.ToString(v, 16);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return result;
        }
    }

    public class RenewableFile
    {
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static byte[] Read(string path)
        {
            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public static async void ReadAysnc(string path, Action<byte[]> callBack)
        {
            byte[] buffer;

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, (int)stream.Length);
            }
            callBack?.Invoke(buffer);
        }

        public static void Write(string path, byte[] buffer)
        {
            string folder = System.IO.Path.GetDirectoryName(path);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
                File.WriteAllBytes(path, buffer);
            }
            catch (Exception e) { Debug.LogError("RenewableResource Write Error: " + e.Message); }
        }

        public static void WriteAysnc(string path, byte[] buffer)
        {
            string folder = System.IO.Path.GetDirectoryName(path);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    stream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
            catch (Exception e) { Debug.LogError("RenewableResource Write Error: " + e.Message); }
        }

        public static void Delete(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}