using System.IO;
using System.Text;

namespace UnityEditor.Listener
{
    public class AssetModificationListener : AssetModificationProcessor
    {
        protected static void OnWillCreateAsset(string path)
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

        protected static string[] OnWillSaveAssets(string[] paths)
        {
            return paths;
        }

        private static void ScriptEncoding(string path)
        {
            if (File.Exists(path))
            {
                File.WriteAllText(path, File.ReadAllText(path), new UTF8Encoding(false));
            }
        }
    }
}