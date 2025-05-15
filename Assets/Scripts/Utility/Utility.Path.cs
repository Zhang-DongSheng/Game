using Game.Const;
using System.IO;
using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        /// <summary>
        /// 路径
        /// </summary>
        public static class Path
        {
            public static string Project
            {
                get
                {
                    return Application.dataPath.Remove(Application.dataPath.Length - 7);
                }
            }

            public static string StreamingAssets
            {
                get
                {
                    return Application.streamingAssetsPath;
                }
            }

            public static string PersistentData
            {
                get
                {
                    return Application.persistentDataPath;
                }
            }

            public static string GetPathWithoutExtension(string path)
            {
                string extension = GetExtension(path);

                if (!string.IsNullOrEmpty(extension))
                {
                    int length = extension.Length;

                    path = path[..^length];
                }
                return path;
            }

            public static string GetDirectoryName(string path)
            {
                if (string.IsNullOrEmpty(path)) return string.Empty;

                return System.IO.Path.GetDirectoryName(path);
            }

            public static string GetFileName(string path)
            {
                if (string.IsNullOrEmpty(path)) return string.Empty;

                return System.IO.Path.GetFileName(path);
            }

            public static string GetFileNameWithoutExtension(string path)
            {
                if (string.IsNullOrEmpty(path)) return string.Empty;

                return System.IO.Path.GetFileNameWithoutExtension(path);
            }

            public static string GetExtension(string path)
            {
                if (string.IsNullOrEmpty(path)) return string.Empty;

                return System.IO.Path.GetExtension(path);
            }

            public static string GetRegularPath(string path)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return string.Empty;
                }
                return path.Replace('\\', '/');
            }

            public static string DirectoryEndWithSeparatorChar(string path)
            {
                if (path.EndsWith(System.IO.Path.DirectorySeparatorChar)) 
                {
                
                }
                else
                {
                    path += System.IO.Path.DirectorySeparatorChar;
                }
                return path;
            }
            /// <summary>
            /// 获取远程格式的路径（带有file:// 或 http:// 前缀）
            /// </summary>
            public static string GetRemotePath(string path)
            {
                path = GetRegularPath(path);

                if (string.IsNullOrEmpty(path))
                {
                    return string.Empty;
                }
                return path.Contains("://") ? path : ("file:///" + path).Replace("file:////", "file:///");
            }

            public static string Replace(string path, string from, string to)
            {
                path = GetRegularPath(path);

                if (path.StartsWith(from))
                {
                    path = path.Replace(from, to);
                }
                return path;
            }

            public static string UnityToSystem(string path)
            {
                if (path.StartsWith("Assets/"))
                {
                    return string.Format("{0}/{1}", Application.dataPath, path[7..]);
                }
                return path;
            }

            public static string SystemToUnity(string path)
            {
                path = string.Format("{0}{1}", AssetPath.Assets , path[Application.dataPath.Length..]);

                path = GetRegularPath(path);

                return path;
            }
            /// <summary>
            /// 创建新文件
            /// </summary>
            public static string NewFile(string path)
            {
                string directory = System.IO.Path.GetDirectoryName(path);

                string file = System.IO.Path.GetFileNameWithoutExtension(path);

                string extension = System.IO.Path.GetExtension(path);

                int index = 0;

                path = string.Format("{0}/{1}_{2}{3}", directory, file, index++, extension);

                while (File.Exists(path))
                {
                    path = string.Format("{0}/{1}_{2}{3}", directory, file, index++, extension);
                }
                return path;
            }
        }
    }
}