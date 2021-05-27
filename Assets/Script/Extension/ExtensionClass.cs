using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static partial class Extension
{
    public static T Clone<T>(this T script) where T : class
    {
        using (Stream stream = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, script);
            stream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(stream) as T;
        }
    }
}