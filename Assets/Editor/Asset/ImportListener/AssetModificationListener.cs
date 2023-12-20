using System.IO;
using System.Text;
using Utility = Game.Utility;

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
                var encoding = Utility.Encode.FileEncoding(path);

                var utf8 = new UTF8Encoding(false);

                if (encoding != utf8 && encoding != Encoding.UTF8)
                {
                    Utility.Encode.Convert(path, encoding, utf8);
                }
            }
        }
    }
}