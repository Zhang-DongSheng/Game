namespace Game.Data
{
    public class Notification
    {
        public NotificationType type;

        public int order;

        public float duration;

        public string content;
    }

    public enum NotificationType
    {
        Notice,
        HorseLamp,
    }
}