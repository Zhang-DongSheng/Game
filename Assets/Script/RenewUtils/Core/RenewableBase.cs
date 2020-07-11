namespace UnityEngine
{
    [DisallowMultipleComponent]
    public abstract class RenewableBase : MonoBehaviour
    {
        [SerializeField] protected StorageClass store;

        [SerializeField] protected CacheType cache;

        protected string key = string.Empty;

        protected abstract DownloadFileType fileType { get; }

        protected void Get(string key, string url, string extra, System.Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (this.key.Equals(key))
            {
                callBack?.Invoke();
            }
            else
            {
                RenewableResource.Instance.Get(key, url, extra, store, fileType, (buffer, content) =>
                {
                    this.key = key; Create(buffer, content); callBack?.Invoke();
                });
            }
        }

        protected abstract void Create(byte[] buffer, Object content);

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
    }

    public enum StorageClass
    {
        None,               //不保存
        Write,              //写入本地
        Cache,              //临时缓存
    }

    public enum DownloadFileType
    {
        None,               //默认
        Image,              //图片
        Audio,              //音频
        Bundle,             //资源
    }

    public enum CacheType
    {
        None,               //常驻资源
        Image_Cover,        //封面
        Audio_Cover,        //封面
    }
}