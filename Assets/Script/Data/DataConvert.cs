using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public static class DataConvert
{
    public static byte[] Serialize<T>(T target) where T : class
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter binary = new BinaryFormatter();
            binary.Serialize(stream, target);
            return stream.ToArray();
        }
    }

    public static T Deserialize<T>(byte[] buffer) where T : class
    {
        using (MemoryStream stream = new MemoryStream(buffer))
        {
            BinaryFormatter binary = new BinaryFormatter();
            return binary.Deserialize(stream) as T;
        }
    }

    public static byte[] StringToByte(string value)
    {
        return Encoding.Default.GetBytes(value);
    }

    public static string ByteToString(byte[] buffer)
    {
        return Encoding.Default.GetString(buffer);
    }
}