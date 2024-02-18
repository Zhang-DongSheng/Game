using Game;
using System.IO;
using System.Text;

namespace UnityEditor.Ebook
{
    public class ConvertFormat
    {
        public void Format(string path, Encoding encoding)
        {
            try
            {
                Encoding decoding = Utility.Encode.FileEncoding(path);

                string content = File.ReadAllText(path);

                path = Utility.Path.NewFile(path);

                File.WriteAllText(path, content, encoding);
            }
            catch
            {

            }
        }
    }
}