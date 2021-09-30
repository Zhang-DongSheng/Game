namespace Data
{
    [System.Serializable]
    public class ActionInformation
    {
        public ActionType type;

        public int count;
    }

    public enum ActionType
    {
        None,
        Cost,
        Kill,
        Talk,
        Time,
    }
}