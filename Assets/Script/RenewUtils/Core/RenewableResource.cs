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
        public int DownloadLimit = 3;

        private string key;

        private string url;

        private string path;

        private readonly Dictionary<string, Task> m_task = new Dictionary<string, Task>();

        private readonly Dictionary<string, Object> m_cache = new Dictionary<string, Object>();

        private RenewableResource() { }

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public void Init()
        {
            //Get the latest resources list ...
            if (true)
            {
                DownloadLimit = 1; RenewableResourceUpdate.Instance.Load("");
            }
        }

        public void Get(string key, string url = "", string extra = "", StorageClass store = StorageClass.None, DownloadFileType fileType = DownloadFileType.None, Action<byte[], Object> callBack = null)
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
            else if (File.Exists(this.path))
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
                    m_task.Add(this.key, new Task(this.key, this.url, path, fileType, store)
                    {
                        callBack = callBack,
                        local = true,
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
                    m_task.Add(this.key, new Task(this.key, this.url, path, fileType, store)
                    {
                        callBack = callBack,
                        local = false,
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
                int index, count = 0;

                foreach (var task in m_task.Values)
                {
                    if (task.status == TaskStatus.Loading)
                    {
                        count++;
                    }
                }

                index = count;

                foreach (var task in m_task.Values)
                {
                    if (index++ < Count)
                    {
                        if (task.status == TaskStatus.Ready)
                        {
                            m_task[task.Key].status = TaskStatus.Loading;

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
            if (m_task.ContainsKey(key))
            {
                UnityWebRequest request = Request(m_task[key].Type, url);

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
                            switch (m_task[key].Store)
                            {
                                case StorageClass.Write:
                                    Write(m_task[key].Path, request.downloadHandler.data);
                                    break;
                                case StorageClass.Cache:
                                    if (m_cache.ContainsKey(key) == false)
                                        m_cache.Add(key, m_task[key].content);
                                    break;
                            }
                            m_task[key].content = DownloadHandler(m_task[key].Type, request);
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
            UnityWebRequest request = Request(m_task[key].Type, path);

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
                        m_task[key].content = DownloadHandler(m_task[key].Type, request);

                        if (RenewableResourceUpdate.Instance.Validation(m_task[key].Key, request.downloadHandler.data))
                        {
                            m_task[key].callBack?.Invoke(request.downloadHandler.data, m_task[key].content);
                            m_task[key].status = TaskStatus.Complete;
                            m_task.Remove(key);
                        }
                        else
                        {
                            Delete(m_task[key].Path);
                            m_task[key].local = false;
                            m_task[key].status = TaskStatus.Ready;
                        }
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

        private void Delete(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
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
                return "https://www.baidu.com/";
            }
        }

        private string Path { get { return Application.persistentDataPath + "/"; } }

        private int Count { get { return DownloadLimit; } }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        class Task
        {
            private readonly string _key;

            private readonly string _url;

            private readonly string _path;

            private readonly DownloadFileType _type;

            private readonly StorageClass _store;

            public TaskStatus status;

            public bool local;

            public Object content;

            public Action<byte[], Object> callBack;

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

    public class RenewableResourceUpdate : Singleton<RenewableResourceUpdate>
    {
        private const string Path = "information/md5file.txt";

        private readonly Dictionary<string, string> m_secret = new Dictionary<string, string>();

        public void Load(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Path;
            }
            RenewableResource.Instance.Get(path, "", "", StorageClass.None, DownloadFileType.None, Load);
        }

        private void Load(byte[] buffer, Object result)
        {
            string content = Encoding.Default.GetString(buffer);

            content = content.Replace("\r\n", "^");

            string[] rows = content.Split('^');

            for (int i = 0; i < rows.Length; i++)
            {
                string[] value = rows[i].Split('|');

                if (value.Length == 2)
                {
                    m_secret.Add(value[0], value[1]);
                }
            }

            RenewableResource.Instance.DownloadLimit = 3;
        }

        public bool Validation(string key, byte[] buffer)
        {
            if (string.IsNullOrEmpty(key) || !m_secret.ContainsKey(key)) return true;

            string md5 = ComputeMD5(buffer);

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
}