using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.Networking;

namespace UnityEngine.Renewable
{
    public class RenewableResource : MonoSingleton<RenewableResource>
    {
        private const string Path_Android = "file://";

        private const int PollingTime = 10;

        public int DownloadLimit = 3;

        private string url, path;

        private float timer;

        private List<Task> tasks;

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
                if (m_task.Count > 0 && NetworkConnection)
                {
                    TryAgainDownload();
                }
                timer = 0;
            }
        }

        public void Init()
        {
            if (true)
            {
                DownloadLimit = 1; RenewableResourceUpdate.Instance.Load();
            }
            RenewableFile.FolderCheck(System.IO.Path.Combine(Application.persistentDataPath, "assets/"));
        }

        public void Get(RenewableRequest request, Action<RenewableDownloadHandler> success = null, Action fail = null)
        {
            if (string.IsNullOrEmpty(request.key)) return;

            if (m_task.ContainsKey(request.key))
            {
                m_task[request.key].AddListener(success, fail);
            }
            else
            {
                url = Url + request.key;

                path = Path + request.key;

                m_task.Add(request.key, new Task(request, url, path)
                {
                    status = TaskStatus.Ready,
                });
                m_task[request.key].AddListener(success, fail);

                Next();
            }
        }

        public void SetOrder(string key, int order)
        {
            if (m_task.ContainsKey(key))
            {
                m_task[key].order = order;
            }
        }

        private void TryAgainDownload()
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

                if (loading < Count)
                {
                    tasks = m_task.Values.ToList<Task>();

                    tasks.Sort((a, b) =>
                    {
                        if (a.order != b.order)
                        {
                            return a.order > b.order ? 1 : -1;
                        }
                        return 0;
                    });

                    for (int i = 0; i < tasks.Count; i++)
                    {
                        switch (tasks[i].status)
                        {
                            case TaskStatus.Ready:
                                if (loading++ < Count)
                                {
                                    tasks[i].status = TaskStatus.Downloading;

                                    //Debug.LogFormat("<color=red>Downloading ... </color><color=green>[{0}]</color><color=blue>[{1}]</color>", tasks[i].Key, tasks[i].Path);

                                    if (tasks[i].local)
                                    {
                                        StartCoroutine(Load(tasks[i].Key, tasks[i].Path));
                                    }
                                    else
                                    {
                                        StartCoroutine(Dowmload(tasks[i].Key, tasks[i].Url));
                                    }
                                }
                                break;
                        }
                        if (loading >= Count) break;
                    }
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
                    if (request.result != UnityWebRequest.Result.Success)
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
                if (request.result != UnityWebRequest.Result.Success)
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
                task.Parameter,
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

        class Task
        {
            private readonly string _key;

            private readonly string _url;

            private readonly string _path;

            private readonly string _parameter;

            private readonly DownloadFileType _type;

            private readonly StorageClass _store;

            public TaskStatus status;

            public int order;

            public bool local;

            public Object content;

            public Action<RenewableDownloadHandler> success;

            public Action fail;

            public string Key { get { return _key; } }

            public string Url { get { return _url; } }

            public string Path { get { return _path; } }

            public string Parameter { get { return _parameter; } }

            public string Secret { get { return string.Format("{0}+{1}+{2}", Key, DateTime.UtcNow.Ticks, Random.Range(0, 100)); } }

            public DownloadFileType Type { get { return _type; } }

            public StorageClass Store { get { return _store; } }

            public Task(RenewableRequest request, string url, string path)
            {
                this._key = request.key;

                this._parameter = request.parameter;

                this._url = url;

                this._path = path;

                this._type = request.type;

                this._store = request.store;

                this.order = request.order;

                this.local = File.Exists(path);
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

    public class RenewableRequest
    {
        public string key;

        public string parameter;

        public int order;

        public StorageClass store;

        public DownloadFileType type;

        public RenewableRequest(string key, string parameter = null, int order = 0, StorageClass store = StorageClass.None, DownloadFileType type = DownloadFileType.None)
        {
            this.key = key;

            this.parameter = parameter;

            this.order = order;

            this.store = store;

            this.type = type;
        }
    }

    public class RenewableDownloadHandler
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
    }
}