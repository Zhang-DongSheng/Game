namespace UnityEngine.Audio
{
    public static class AudioConfig
    {
        public static bool mute = false;

        public static float volum = 1f;
    }

    public class AudioInformation
    {
        public string key;

        public string path;
    }

    public enum AudioEnum
    {
        Music,
        Effect,
        Special,
    }
}