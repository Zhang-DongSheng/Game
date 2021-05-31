using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UnityEngine.Encrypt
{
    public static class FileEncrypt
    {
        private const string AESKEY32 = "abcdefghijklmnopqrstuvwxyz123456";

        private const string AESIV16 = "abcdefghijklmnop";

        private const string DESKEY8 = "abcdefgh";

        private const string DESIV8 = "abcdefgh";

        public static string Encrypt(string value, EncryptType encrypt = EncryptType.AES)
        {
            switch (encrypt)
            {
                case EncryptType.AES:
                    return AESEncrypt(value);
                case EncryptType.DES:
                    return DESEncrypt(value);
                default:
                    return value;
            }
        }

        public static string Decrypt(string value, EncryptType encrypt = EncryptType.AES)
        {
            switch (encrypt)
            {
                case EncryptType.AES:
                    return AESDecrypt(value);
                case EncryptType.DES:
                    return DESDecrypt(value);
                default:
                    return value;
            }
        }

        private static byte[] AESKEY { get { return Encoding.Default.GetBytes(AESKEY32); } }

        private static byte[] AESIV { get { return Encoding.Default.GetBytes(AESIV16); } }

        private static string AESEncrypt(string value)
        {
            byte[] buffer = Encoding.Default.GetBytes(value);

            using (RijndaelManaged managed = new RijndaelManaged())
            {
                ICryptoTransform crypt = managed.CreateEncryptor(AESKEY, AESIV);
                value = Convert.ToBase64String(crypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            return value;
        }

        private static string AESDecrypt(string value)
        {
            byte[] buffer = Convert.FromBase64String(value);

            using (RijndaelManaged managed = new RijndaelManaged())
            {
                ICryptoTransform crypt = managed.CreateDecryptor(AESKEY, AESIV);
                value = Encoding.Default.GetString(crypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            return value;
        }

        private static byte[] DESKEY { get { return Encoding.Default.GetBytes(DESKEY8); } }

        private static byte[] DESIV { get { return Encoding.Default.GetBytes(DESIV8); } }

        private static string DESEncrypt(string value)
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

        private static string DESDecrypt(string value)
        {
            byte[] buffer = Convert.FromBase64String(value);

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

            MemoryStream memory = new MemoryStream(buffer);

            CryptoStream crypto = new CryptoStream(memory, provider.CreateDecryptor(DESKEY, DESIV), CryptoStreamMode.Read);

            StreamReader reader = new StreamReader(crypto);

            return reader.ReadToEnd();
        }
    }

    public enum EncryptType
    {
        AES,
        DES,
    }
}