using System.Text;

namespace Game.Network
{
    public static class Convert
    {
        public static string ToString(byte[] buffer)
        {
            return Encoding.UTF32.GetString(buffer);
        }

        public static string ToString(byte[] buffer, int index, int count)
        {
            return Encoding.UTF32.GetString(buffer, index, count);
        }

        public static byte[] ToBytes(string value)
        {
            return Encoding.UTF32.GetBytes(value);
        }
    }
}