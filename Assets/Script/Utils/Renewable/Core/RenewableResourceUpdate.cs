using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace UnityEngine.Renewable
{
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
            RenewableResource.Instance.Get(new RenewableRequest(path), success: Load);
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
}