using Game;
using System.IO;
using System.Text;
using Utils;

namespace UnityEditor.Ebook
{
    public class ConvertFormat
    {
        public void Format(string path, Encoding encoding)
        {
            try
            {
                Encoding decoding = Utility._Encode.FileEncoding(path);

                string content = File.ReadAllText(path);

                path = Utility._Path.New(path);

                File.WriteAllText(path, content, encoding);
            }
            catch
            {

            }
        }
    }
}