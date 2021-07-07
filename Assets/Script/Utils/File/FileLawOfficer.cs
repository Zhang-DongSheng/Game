using System.Collections;
using System.IO;
using System.Text;
using UnityEngine.Networking;

namespace UnityEngine
{
    public class FileLawOfficer : MonoBehaviour
    {
        [SerializeField] private string url;

        private void Awake()
        {
            StartCoroutine(Download(url));
        }

        IEnumerator Download(string url)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (request.isDone)
            {
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Handle(request.downloadHandler.data);
                }
                else
                {
                    Debug.Log("Law officer can't find file!");
                }
            }
        }

        private void Handle(byte[] buffer)
        {
            string content = Encoding.Default.GetString(buffer);

            if (!string.IsNullOrEmpty(content))
            {
                content = content.Replace("\r\n", "^");

                string[] values = content.Split('^');

                for (int i = 0; i < values.Length; i++)
                {
                    Excute(values[i]);
                }
            }
        }

        private void Excute(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            if (value.StartsWith("%0"))
            {
#if UNITY_ANDROID
                value = value.Replace("%0", "android");
#elif UNITY_IOS
                value = value.Replace("%0", "ios");
#endif
            }
            value = string.Format("{0}/{1}", Application.persistentDataPath, value);

            Delete(value);
        }

        private void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}