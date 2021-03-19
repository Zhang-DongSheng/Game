namespace UnityEngine.Audio
{
    public static class AudioConfig
    {
        public static bool mute = false;

        public static float volum = 1f;
    }

    public enum SourceEnum
    {
        Music,
        Effect,
    }

    public enum ListenerEnum
    {
        Environment,
        Personal,
    }
}