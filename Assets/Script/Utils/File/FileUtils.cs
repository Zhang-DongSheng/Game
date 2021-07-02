using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Encrypt;

public static class FileUtils
{
    public static void FolderCreate(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
            Directory.CreateDirectory(path);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static void FolderDelete(string path, bool recursive = true)
    {
        if (Directory.Exists(path))
        {
            try
            {
                Directory.Delete(path, recursive);
            }
            catch (Exception e)
            {
                throw e;
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

            content = FileEncrypt.Encrypt(Encoding.Default.GetString(buffer));
        }
        return content;
    }

    public static void Copy(string from, string to)
    {
        if (File.Exists(from))
        {
            string folder = Path.GetDirectoryName(to);

            if (Directory.Exists(folder) == false)
            {
                Directory.CreateDirectory(folder);
            }
            File.Copy(from, to, true);
        }
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
            byte[] buffer = Encoding.Default.GetBytes(FileEncrypt.Decrypt(content));

            if (Directory.Exists(folder) == false)
            {
                Directory.CreateDirectory(folder);
            }
            File.WriteAllBytes(path, buffer);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static string UnityToSystem(string path)
    {
        if (path.StartsWith("Assets/"))
        {
            return string.Format("{0}{1}", Application.dataPath, path.Remove(0, 6));
        }
        return path;
    }

    public static string SystemToUnity(string path)
    {
        path = string.Format("Assets{0}", path.Remove(0, Application.dataPath.Length));

        path = path.Replace('\\', '/');

        return path;
    }
}