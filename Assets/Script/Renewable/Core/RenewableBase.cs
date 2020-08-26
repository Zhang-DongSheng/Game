using System;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    public abstract class RenewableBase : MonoBehaviour
    {
        [SerializeField] protected StorageClass store;

        [SerializeField] protected RPKey cache;

        protected string key = string.Empty;

        protected string current;

        protected abstract DownloadFileType fileType { get; }

        protected void Get(string key, string parameter, int order, Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (this.key.Equals(key))
            {
                callBack?.Invoke();
            }
            else
            {
                RenewableResource.Instance.Get(new RenewableRequest(key, parameter, order, store, fileType), (handle) =>
                {
                    Create(handle);

                    if (this != null)
                    {
                        if (string.IsNullOrEmpty(current) || current == handle.key)
                        {
                            this.key = handle.key; callBack?.Invoke();
                        }
                    }
                }, Fail);
            }
        }

        protected abstract void Create(RenewableDownloadHandler handle);

        protected virtual void Fail()
        { 
            
        }

        public virtual void ResetRenewable()
        {
            this.key = string.Empty;
        }

        public void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }

        protected bool TryLoad<T>(string path, out T source) where T : Object
        {
            source = Resources.Load<Object>(path) as T;

            if (source != null)
            {
                return true;
            }
            return false;
        }
    }

    public enum StorageClass
    {
        None,               //不保存
        Write,              //写入本地
    }

    public enum DownloadFileType
    {
        None,               //默认
        Image,              //图片
        Audio,              //音频
        Bundle,             //资源
    }
}