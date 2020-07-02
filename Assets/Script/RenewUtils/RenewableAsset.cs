using System;

namespace UnityEngine.UI
{
    public class RenewableAsset : RenewableBase
    {
        [SerializeField] private Transform parent;

        private GameObject target;

        private string param = string.Empty;

        protected override DownloadFileType fileType { get { return DownloadFileType.Bundle; } }

        public void Create(string key, string param, string url = "", string extra = "", Action callBack = null)
        {
            Get(key, param, url, extra, "", callBack);
        }

        private void Get(string key, string param, string url, string extra, string md5, Action callBack = null)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(param)) return;

            if (this.key.Equals(key) && this.param.Equals(param))
            {
                callBack?.Invoke();
            }
            else
            {
                if (target != null) Destroy(target);

                RenewableResource.Instance.Get(key, url, extra, md5, store, fileType, (buffer, content) =>
                {
                    this.key = key; this.param = param;
                    Create(buffer, content); callBack?.Invoke();
                });
            }
        }

        protected override void Create(byte[] buffer, Object content)
        {
            Instantiate(Prefab(buffer, param));
        }

        private GameObject Prefab(byte[] buffer, string name)
        {
            GameObject result = null;

            try
            {
                AssetBundle asset = AssetBundle.LoadFromMemory(buffer);
                result = asset.LoadAsset<GameObject>(name);
                asset.Unload(false);
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
            }

            return result;
        }

        private void Instantiate(GameObject prefab)
        {
            if (prefab == null) return;

            target = Instantiate(prefab, parent);

            target.transform.localRotation = Quaternion.identity;
            target.transform.localPosition = Vector3.zero;

            target.SetActive(true);
        }
    }
}