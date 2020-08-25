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
        private const string Path_Android = "file://";

        private const int PollingTime = 10;

        public int DownloadLimit = 3;

        private string key;

        private string url;

        private string path;

        private float timer;

        private readonly Dictionary<string, Task> m_task = new Dictionary<string, Task>();

        private RenewableResource() { }

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > PollingTime)
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

        public void Get(string key, string url = "", string parameter = "", StorageClass store = StorageClass.None, DownloadFileType fileType = DownloadFileType.None, Action<RenewableDownloadHandler> success = null, Action fail = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.key = key;

            this.url = string.IsNullOrEmpty(url) ? Url : url;

            this.url += this.key;

            this.path = Path + this.key;

            if (m_task.ContainsKey(this.key))
            {
                m_task[this.key].AddListener(success, fail);
            }
            else
            {
                m_task.Add(this.key, new Task(this.key, this.url, path, fileType, store)
                {
                    parameter = parameter,
                    local = File.Exists(this.path),
                    status = TaskStatus.Ready,
                });
                m_task[this.key].AddListener(success, fail);
                Next();
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
                            RenewableResourceUpdate.Instance.Remove(m_task[key].Key);
                            m_task[key].content = DownloadHandler(m_task[key].Type, request);
                            m_task[key].success?.Invoke(Handler(m_task[key], request, true));
                            m_task[key].status = TaskStatus.Complete;
                            switch (m_task[key].Store)
                            {
                                case StorageClass.Write:
                                    if (m_task[key].Type != DownloadFileType.Bundle)
                                        RenewableFile.Write(m_task[key].Path, request.downloadHandler.data);
                                    break;
                            }
                            m_task.Remove(key);
                        }
                    }
                    request.Dispose(); Next();
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

                        if (RenewableResourceUpdate.Instance.Validation(m_task[key].Key, request.downloadHandler.data))
                        {
                            RenewableResourceUpdate.Instance.Remove(m_task[key].Key);
                            m_task[key].success?.Invoke(Handler(m_task[key], request, true));
                            m_task[key].status = TaskStatus.Complete;
                            m_task.Remove(key);
                        }
                        else
                        {
                            m_task[key].success?.Invoke(Handler(m_task[key], request, false));
                            m_task[key].local = false;
                            m_task[key].status = TaskStatus.Ready;
                        }
                    }
                }
                request.Dispose(); Next();
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

        private RenewableDownloadHandler Handler(Task task, UnityWebRequest request, bool recent)
        {
            return new RenewableDownloadHandler(
                task.Key,
                task.parameter,
                task.Secret, recent,
                task.Type != DownloadFileType.Bundle ? request.downloadHandler.data : null,
                task.content);
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
                return "https://www.baidu.com";
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

            public string parameter;

            public Object content;

            public Action<RenewableDownloadHandler> success;

            public Action fail;

            public string Key { get { return _key; } }

            public string Url { get { return _url; } }

            public string Path { get { return _path; } }

            public string Secret { get { return string.Format("{0}+{1}+{2}", Key, DateTime.UtcNow.Ticks, Random.Range(0, 100)); } }

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

            public void AddListener(Action<RenewableDownloadHandler> success, Action fail)
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

            public void SetOrder(int order)
            {
                this.order = order;
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

    public class RenewableDownloadHandler : IDisposable
    {
        public string key;

        public string parameter;

        public string secret;

        public bool recent;

        public byte[] buffer;

        public Object source;

        public RenewableDownloadHandler(string key, string parameter, string secret, bool recent, byte[] buffer, Object source)
        {
            this.key = key;

            this.parameter = parameter;

            this.secret = secret;

            this.recent = recent;

            this.buffer = buffer;

            this.source = source;
        }

        public T Get<T>() where T : Object
        {
            return source as T;
        }

        public void Dispose()
        {
            
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

        private void Load(RenewableDownloadHandler handler)
        {
            string content = Encoding.Default.GetString(handler.buffer);

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

        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (m_secret.ContainsKey(key))
            {
                m_secret.Remove(key);
            }
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

        public static void Delete(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}