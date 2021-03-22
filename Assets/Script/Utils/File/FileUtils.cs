using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class FileUtils
{
    public static void CreateFolder(string path)
    {
        if (Directory.Exists(path)) return;

        try
        {
            Directory.CreateDirectory(path);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public static void DeleteFolder(string path, bool recursive = true)
    {
        if (Directory.Exists(path))
        {
            try
            {
                Directory.Delete(path, recursive);
            }
            catch (Exception e)
            {
                Debug.Log("File Error :" + e.Message);
            }
        }
    }

    public static float SizeOfFolder(string path)
    {
        float size = 0;

        if (Directory.Exists(path))
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            foreach (FileInfo file in dir.GetFiles())
            {
                size += file.Length;
            }
        }

        return size;
    }

    public static string FormatSize(float length)
    {
        int KB = 1024;

        int MB = KB * 1024;

        int GB = MB * 1024;

        string result;

        if (length > GB)
        {
            result = string.Format("{0}GB", (length / GB).ToString("F2"));
        }
        else if (length > MB)
        {
            result = string.Format("{0}MB", (length / MB).ToString("F2"));
        }
        else if (length > KB)
        {
            result = string.Format("{0}KB", (length / KB).ToString("F2"));
        }
        else
        {
            result = string.Format("{0}B", length);
        }

        return result;
    }

    public static string MD5(string path)
    {
        string result = string.Empty;

        try
        {
            if (File.Exists(path))
            {
                byte[] buffer = File.ReadAllBytes(path);

                MD5 md5 = new MD5CryptoServiceProvider();

                byte[] hash = md5.ComputeHash(buffer);

                foreach (byte v in hash)
                {
                    result += Convert.ToString(v, 16);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        return result;
    }

    public static string Read(string path)
    {
        string content = string.Empty;

        if (File.Exists(path))
        {
            content = File.ReadAllText(path);
        }
        return content;
    }

    public static string ReadEncrypt(string path)
    {
        string content = string.Empty;

        if (File.Exists(path))
        {
            byte[] buffer = File.ReadAllBytes(path);
            buffer = FileEncrypt.DecryptBytes(buffer);
            content = Encoding.Default.GetString(buffer);
        }
        return content;
    }

    public static void Write(string path, string content)
    {
        string folder = Path.GetDirectoryName(path);

        try
        {
            if (Directory.Exists(folder) == false)
            {
                Directory.CreateDirectory(folder);
            }
            File.WriteAllText(path, content);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static void WriteEncrypt(string path, string content)
    {
        string folder = Path.GetDirectoryName(path);

        try
        {
            if (Directory.Exists(folder) == false)
            {
                Directory.CreateDirectory(folder);
            }
            byte[] buffer = Encoding.Default.GetBytes(content);
            buffer = FileEncrypt.EncryptBytes(buffer);
            File.WriteAllBytes(path, buffer);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}