using System.IO;
using UnityEngine;

namespace Game
{
    public static partial class Utility
    {
        public static class Path
        {
            /// <summary>
            /// 获取路径
            /// </summary>
            public static string GetDirectoryName(string path)
            {
                return System.IO.Path.GetDirectoryName(path);
            }
            /// <summary>
            /// 获取后缀
            /// </summary>
            public static string GetExtension(string path)
            {
                return System.IO.Path.GetExtension(path);
            }
            /// <summary>
            /// 获取文件名
            /// </summary>
            public static string GetFileName(string path)
            {
                return System.IO.Path.GetFileName(path);
            }
            /// <summary>
            /// 获取不带后缀文件名
            /// </summary>
            public static string GetFileNameWithoutExtension(string path)
            {
                return System.IO.Path.GetFileNameWithoutExtension(path);
            }
            /// <summary>
            /// 获取规范的路径。
            /// </summary>
            public static string GetRegularPath(string path)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return string.Empty;
                }
                return path.Replace('\\', '/');
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
                    return string.Format("{0}{1}", Application.dataPath, path.Remove(0, 6));
                }
                return path;
            }

            public static string SystemToUnity(string path)
            {
                path = string.Format("Assets{0}", path.Remove(0, Application.dataPath.Length));

                path = GetRegularPath(path);

                return path;
            }
            /// <summary>
            /// 创建新文件
            /// </summary>
            public static string New(string path)
            {
                string directory = Path.GetDirectoryName(path);

                string file = Path.GetFileNameWithoutExtension(path);

                string extension = Path.GetExtension(path);

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