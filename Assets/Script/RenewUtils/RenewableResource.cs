using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;

namespace UnityEngine
{
    public class RenewableResource : MonoBehaviour
    {
        private static RenewableResource _instance;
        public static RenewableResource Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("ResourceHelper").AddComponent<RenewableResource>();
                }
                return _instance;
            }
        }

        private string key;

        private string url;

        private string path;

        private readonly Dictionary<string, Task> m_task = new Dictionary<string, Task>();

        private readonly Dictionary<string, Object> m_cache = new Dictionary<string, Object>();

        private RenewableResource() { }

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void Get(string key, string url = "", string extra = "", string md5 = "", StorageClass store = StorageClass.None, DownloadFileType fileType = DownloadFileType.None, Action<byte[], Object> callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            this.key = key;

            this.url = string.IsNullOrEmpty(url) ? Url : url;

            this.url += this.key;

            this.path = Path + extra + this.key;

            if (m_cache.ContainsKey(this.key))
            {
                callBack?.Invoke(null, m_cache[this.key]);
            }
            else if (File.Exists(this.path) && FileUtils.MD5Validation(this.path, md5))
            {
                if (m_task.ContainsKey(this.key))
                {
                    m_task[this.key].callBack += callBack;
                }
                else
                {
#if UNITY_EDITOR

#elif UNITY_ANDROID
                    path = "file://" + path;
#else
                    path = "file://" + path;
#endif
                    m_task.Add(this.key, new Task()
                    {
                        key = this.key,
                        path = path,
                        local = true,
                        type = fileType,
                        callBack = callBack,
                        status = TaskStatus.Ready,
                    });
                    Next();
                }
            }
            else
            {
                if (m_task.ContainsKey(this.key))
                {
                    m_task[this.key].callBack += callBack;
                }
                else
                {
                    m_task.Add(this.key, new Task()
                    {
                        key = this.key,
                        url = this.url,
                        path = path,
                        local = false,
                        store = store,
                        type = fileType,
                        callBack = callBack,
                        status = TaskStatus.Ready,
                    });
                    Next();
                }
            }
        }

        private void Next()
        {
            if (m_task.Count > 0)
            {
                int index = 0;

                foreach (var task in m_task.Values)
                {
                    if (index++ < Count)
                    {
                        if (task.status == TaskStatus.Ready)
                        {
                            m_task[task.key].status = TaskStatus.Loading;

                            //Debug.LogFormat("Downloading ... [{0}][{1}]", task.key, task.path);

                            if (task.local)
                            {
                                StartCoroutine(Load(task.key, task.path));
                            }
                            else
                            {
                                StartCoroutine(Dowmload(task.key, task.url));
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private IEnumerator Dowmload(string key, string url)
        {
            if(m_task.ContainsKey(key))
            {
                UnityWebRequest request = Request(m_task[key].type, url);

                yield return request.SendWebRequest();

                if (request.isDone)
                {
                    if (request.isHttpError || request.isNetworkError)
                    {
                        if (m_task.ContainsKey(key))
                            m_task[key].status = TaskStatus.Fail;
                        Debug.LogWarningFormat("{0}, url:{1}", request.error, url);
                    }
                    else
                    {
                        if (m_task.ContainsKey(key))
                        {
                            switch (m_task[key].store)
                            {
                                case StorageClass.Write:
                                    Write(m_task[key].path, request.downloadHandler.data);
                                    break;
                                case StorageClass.Cache:
                                    if (m_cache.ContainsKey(key) == false)
                                        m_cache.Add(key, m_task[key].content);
                                    break;
                            }
                            m_task[key].content = DownloadHandler(m_task[key].type, request);
                            m_task[key].callBack?.Invoke(request.downloadHandler.data, m_task[key].content);
                            m_task[key].status = TaskStatus.Complete;
                            m_task.Remove(key);
                        }
                    }
                    Next();
                }
            }
        }

        private IEnumerator Load(string key, string path)
        {
            UnityWebRequest request = Request(m_task[key].type, path);

            yield return request.SendWebRequest();

            if (request.isDone)
            {
                if (request.isHttpError || request.isNetworkError)
                {
                    if (m_task.ContainsKey(key))
                        m_task[key].status = TaskStatus.Fail;
                    Debug.LogWarningFormat("{0}, url:{1}", request.error, path);
                }
                else
                {
                    if (m_task.ContainsKey(key))
                    {
                        m_task[key].content = DownloadHandler(m_task[key].type, request);
                        m_task[key].callBack?.Invoke(request.downloadHandler.data, m_task[key].content);
                        m_task[key].status = TaskStatus.Complete;
                        m_task.Remove(key);
                    }
                }
                Next();
            }
        }

        private byte[] Read(string path)
        {
            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        private void Write(string path, byte[] buffer)
        {
            string folder = System.IO.Path.GetDirectoryName(path);
            if (Directory.Exists(folder) == false)
                Directory.CreateDirectory(folder);
            if (File.Exists(path))
                File.Delete(path);
            File.WriteAllBytes(path, buffer);
        }

        private UnityWebRequest Request(DownloadFileType fileType, string url)
        {
            switch (fileType)
            {
                case DownloadFileType.Image:
                    return UnityWebRequestTexture.GetTexture(url);
                case DownloadFileType.Audio:
#if UNITY_EDITOR && !UNITY_ANDROID
                    return UnityWebRequest.Get(url);
#else
                    return UnityWebRequestMultimedia.GetAudioClip(url, GetAudioType(url));
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
#if UNITY_EDITOR && !UNITY_ANDROID
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

        private AudioType GetAudioType(string url)
        {
            AudioType audioType;

            string[] param = url.Split('.');

            string suffix = param.Length > 1 ? param[param.Length - 1].ToLower() : string.Empty;

            switch (suffix)
            {
                case "mp3":
#if UNITY_ANDROID
                    audioType =  AudioType.MPEG;
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
                return GameConfig.ResourceServerURL;
            }
        }

        private string Path { get { return Application.persistentDataPath + "/"; } }

        private int Count { get { return 3; } }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        class Task
        {
            public string key;

            public string url;

            public string path;

            public bool local;

            public DownloadFileType type;

            public StorageClass store;

            public TaskStatus status;

            public Object content;

            public Action<byte[], Object> callBack;
        }

        enum TaskStatus
        {
            Ready,
            Loading,
            Complete,
            Fail,
        }
    }

    public class RenewablePool : Singleton<RenewablePool>
    {
        public readonly Dictionary<string, Object> asset = new Dictionary<string, Object>();

        public void Push(string key, Object value)
        {
            if (asset.ContainsKey(key))
            {
                asset[key] = value;
            }
            else
            {
                asset.Add(key, value);
            }
        }

        public T Pop<T>(string key) where T : Object
        {
            T result = null;

            if (asset.ContainsKey(key))
            {
                result = asset[key] as T;
            }

            return result;
        }

        public bool Exist(string key) { return asset.ContainsKey(key); }
    }
}