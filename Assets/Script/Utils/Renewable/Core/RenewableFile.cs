using System;
using System.IO;

namespace UnityEngine.Renewable
{
    public static class RenewableFile
    {
        public static void FolderCheck(string folder)
        {
            folder = Path.GetDirectoryName(folder);

            if (!Directory.Exists(folder))
            {
                if (File.Exists(folder))
                {
                    File.Delete(folder);
                }
                Directory.CreateDirectory(folder);
            }
        }

        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static byte[] Read(string path)
        {
            if (File.Exists(path))
                return File.ReadAllBytes(path);
            else
                return null;
        }

        public static async void ReadAysnc(string path, Action<byte[]> callBack)
        {
            byte[] buffer;

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, (int)stream.Length);
            }
            callBack?.Invoke(buffer);
        }

        public static void Write(string path, byte[] buffer)
        {
            string folder = Path.GetDirectoryName(path);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
                File.WriteAllBytes(path, buffer);
            }
            catch (Exception e) { Debug.LogError("RenewableResource Write Error: " + e.Message); }
        }

        public static void Delete(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}