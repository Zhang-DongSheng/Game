using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Game
{
    public static partial class Utility
    {
        public static class _Cryptogram
        {
            private const string AESKEY32 = "abcdefghijklmnopqrstuvwxyz123456";

            private const string AESIV16 = "abcdefghijklmnop";

            private const string DESKEY8 = "abcdefgh";

            private const string DESIV8 = "abcdefgh";

            private const string RSAKEY = "abcdefgh";

            private const string BASE64KEY = "test";
            /// <summary>
            /// º”√‹
            /// </summary>
            public static string Encrypt(string value, EncryptType encrypt = EncryptType.AES)
            {
                if (string.IsNullOrEmpty(value)) return null;

                switch (encrypt)
                {
                    case EncryptType.AES:
                        return EncryptAES(value);
                    case EncryptType.DES:
                        return EncryptDES(value);
                    case EncryptType.RSA:
                        return EncryptRSA(value);
                    case EncryptType.Base64:
                        return EncryptBase64(value);
                    default:
                        return value;
                }
            }
            /// <summary>
            /// Ω‚√‹
            /// </summary>
            public static string Decrypt(string value, EncryptType encrypt = EncryptType.AES)
            {
                if (string.IsNullOrEmpty(value)) return null;

                switch (encrypt)
                {
                    case EncryptType.AES:
                        return DecryptAES(value);
                    case EncryptType.DES:
                        return DecryptDES(value);
                    case EncryptType.RSA:
                        return DecryptRSA(value);
                    case EncryptType.Base64:
                        return DecryptBase64(value);
                    default:
                        return value;
                }
            }

            private static string EncryptAES(string value)
            {
                byte[] buffer = Encoding.Default.GetBytes(value);

                using (RijndaelManaged managed = new RijndaelManaged())
                {
                    ICryptoTransform crypt = managed.CreateEncryptor(AESKEY, AESIV);

                    value = Convert.ToBase64String(crypt.TransformFinalBlock(buffer, 0, buffer.Length));
                }
                return value;
            }

            private static string DecryptAES(string value)
            {
                byte[] buffer = Convert.FromBase64String(value);

                using (RijndaelManaged managed = new RijndaelManaged())
                {
                    ICryptoTransform crypt = managed.CreateDecryptor(AESKEY, AESIV);

                    value = Encoding.Default.GetString(crypt.TransformFinalBlock(buffer, 0, buffer.Length));
                }
                return value;
            }

            private static string EncryptDES(string value)
            {
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

                MemoryStream memory = new MemoryStream();

                CryptoStream crypto = new CryptoStream(memory, provider.CreateEncryptor(DESKEY, DESIV), CryptoStreamMode.Write);

                StreamWriter writer = new StreamWriter(crypto);

                writer.Write(value);

                writer.Flush();

                crypto.FlushFinalBlock();

                writer.Flush();

                return Convert.ToBase64String(memory.GetBuffer(), 0, (int)memory.Length);
            }

            private static string DecryptDES(string value)
            {
                byte[] buffer = Convert.FromBase64String(value);

                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

                MemoryStream memory = new MemoryStream(buffer);

                CryptoStream crypto = new CryptoStream(memory, provider.CreateDecryptor(DESKEY, DESIV), CryptoStreamMode.Read);

                StreamReader reader = new StreamReader(crypto);

                return reader.ReadToEnd();
            }

            private static string EncryptRSA(string value)
            {
                byte[] buffer = Encoding.Default.GetBytes(value);

                CspParameters parameters = new CspParameters()
                {
                    KeyContainerName = RSAKEY,
                };
                using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider(parameters))
                {
                    buffer = provider.Encrypt(buffer, false);
                }
                return Convert.ToBase64String(buffer);
            }

            private static string DecryptRSA(string value)
            {
                byte[] buffer = Convert.FromBase64String(value);

                CspParameters parameters = new CspParameters()
                {
                    KeyContainerName = RSAKEY,
                };
                using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider(parameters))
                {
                    buffer = provider.Decrypt(buffer, false);
                }
                return Encoding.Default.GetString(buffer);
            }

            private static string EncryptBase64(string value)
            {
                if (value.StartsWith(BASE64KEY))
                {
                    value = value.Remove(0, BASE64KEY.Length);

                    byte[] buffer = Convert.FromBase64String(value);

                    return Encoding.Default.GetString(buffer);
                }
                else
                {
                    return value;
                }
            }

            private static string DecryptBase64(string value)
            {
                value = Convert.ToBase64String(Encoding.Default.GetBytes(value));

                return string.Format("{0}{1}", BASE64KEY, value);
            }

            private static byte[] AESKEY { get { return Encoding.Default.GetBytes(AESKEY32); } }

            private static byte[] AESIV { get { return Encoding.Default.GetBytes(AESIV16); } }

            private static byte[] DESKEY { get { return Encoding.Default.GetBytes(DESKEY8); } }

            private static byte[] DESIV { get { return Encoding.Default.GetBytes(DESIV8); } }
        }

        public enum EncryptType
        {
            AES,
            DES,
            RSA,
            Base64,
        }
    }
}