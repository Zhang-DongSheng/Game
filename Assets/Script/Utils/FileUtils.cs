using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public static class FileUtils
{
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

    public static float FolderSize(string path)
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

    public static string SizeFormat(float length)
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

    public static bool MD5Validation(string path, string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return true;
        }
        else
        {
            return FileMD5(path) == code;
        }
    }

    public static string FileMD5(string path)
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
}
