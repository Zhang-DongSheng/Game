using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UnityEngine.Renewable
{
    public class RenewableResourceUpdate : Singleton<RenewableResourceUpdate>
    {
        private readonly Dictionary<string, string> m_secret = new Dictionary<string, string>();

        public void Load()
        {
            string lawofficer = "information/lawofficer.txt";

            string secret = "information/md5file.txt";

            RenewableResource.Instance.Get(new RenewableRequest(lawofficer), success: LawOfficerHandle);

            RenewableResource.Instance.Get(new RenewableRequest(secret), success: SecretHandle, fail: DoneHandle);
        }

        public bool Validation(string key, byte[] buffer)
        {
            if (string.IsNullOrEmpty(key) || !m_secret.ContainsKey(key)) return true;

            string md5 = buffer != null ? Md5(buffer) : string.Empty;

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

        private void LawOfficerHandle(RenewableDownloadHandler handler)
        {
            string content = Encoding.Default.GetString(handler.buffer);

            content = content.Replace("\r\n", "^");

            string[] values = content.Split('^');

            for (int i = 0; i < values.Length; i++)
            {
                Excute(values[i]);
            }
        }

        private void SecretHandle(RenewableDownloadHandler handler)
        {
            string content = Encoding.Default.GetString(handler.buffer);

            content = content.Replace("\r\n", "^");

            string[] values = content.Split('^');

            for (int i = 0; i < values.Length; i++)
            {
                string[] parameter = values[i].Split('|');

                if (parameter.Length == 2 && !string.IsNullOrEmpty(parameter[0]))
                {
                    if (m_secret.ContainsKey(parameter[0]))
                    {
                        m_secret[parameter[0]] = parameter[1];
                    }
                    else
                    {
                        m_secret.Add(parameter[0], parameter[1]);
                    }
                }
            }
        }

        private void DoneHandle()
        {
            RenewableResource.Instance.DownloadLimit = 3;
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

        private string Md5(byte[] buffer)
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