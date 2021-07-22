using System.IO;
using System.Text;
using Utils;

namespace UnityEditor.Listener
{
    public class AssetModificationListener : AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            if (path.EndsWith(".meta"))
            {
                path = path.Replace(".meta", null);
            }

            string extension = Path.GetExtension(path);

            switch (extension)
            {
                case ".cs":
                    ScriptEncoding(path);
                    break;
                default:
                    break;
            }
        }

        private static void ScriptEncoding(string path)
        {
            Encoding encoding = FileEncoding.Get(path);

            if (encoding == Encoding.ASCII)
            {
                File.WriteAllText(path, File.ReadAllText(path), Encoding.Default);
            }
        }
    }
}