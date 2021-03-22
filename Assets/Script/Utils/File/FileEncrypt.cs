using System.Security.Cryptography;

public static class FileEncrypt
{
    private const bool encrypt = true;

    private static readonly byte[] cryptKey = new byte[] { 0xfe, 0xcc, 0xaa, 0xec, 0xbb, 0x12, 0x01, 0x4c, 0xee, 0x56, 0x0a, 0x13, 0x2b, 0x1a, 0xb3, 0xeb, 0xfe, 0x2c, 0xfa, 0x3c, 0xcb, 0x1d, 0xe1, 0x4c, 0xe3, 0x16, 0x09, 0x18, 0x27, 0x6a, 0xb4, 0x2b };

    private static readonly byte[] cryptIV = new byte[] { 0xff, 0xaa, 0xbb, 0x01, 0xee, 0x0a, 0x2b, 0xc1, 0xef, 0xca, 0xe4, 0x31, 0xef, 0xaa, 0xbc, 0x22 };

    public static byte[] EncryptBytes(byte[] buffer)
    {
        if (!encrypt) return buffer;

        byte[] encrypted = null;

        using (RijndaelManaged managed = new RijndaelManaged())
        {
            ICryptoTransform crypt = managed.CreateEncryptor(cryptKey, cryptIV);
            encrypted = crypt.TransformFinalBlock(buffer, 0, buffer.Length);
        }
        return encrypted;
    }

    public static byte[] DecryptBytes(byte[] buffer)
    {
        if (!encrypt) return buffer;

        byte[] decrypted = null;

        using (RijndaelManaged rijAlg = new RijndaelManaged())
        {
            ICryptoTransform crypt = rijAlg.CreateDecryptor(cryptKey, cryptIV);
            decrypted = crypt.TransformFinalBlock(buffer, 0, buffer.Length);
        }
        return decrypted;
    }
}