namespace UnityEngine
{
    public abstract class RenewableBase : MonoBehaviour
    {
        [SerializeField] protected StorageClass store;

        protected string key = string.Empty;

        protected abstract DownloadFileType fileType { get; }

        protected void Get(string key, string url, string extra, string md5, System.Action callBack = null)
        {
            if (string.IsNullOrEmpty(key)) return;

            if (this.key.Equals(key))
            {
                callBack?.Invoke();
            }
            else
            {
                RenewableResource.Instance.Get(key, url, extra, md5, store, fileType, (buffer, content) =>
                {
                    this.key = key; Create(buffer, content); callBack?.Invoke();
                });
            }
        }

        protected abstract void Create(byte[] buffer, Object content);

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
        None,
        Write,
        Cache,
    }

    public enum DownloadFileType
    {
        None,
        Image,
        Audio,
        Bundle,
    }
}