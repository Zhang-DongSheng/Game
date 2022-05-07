using System.Collections.Generic;

namespace Data
{
    public class DataTask : DataBase
    {
        public List<TaskInformation> tasks = new List<TaskInformation>();

        public TaskInformation Get(int key, bool quick = false)
        {
            return tasks.Find(x => x.primary == key);
        }
    }
    [System.Serializable]
    public class TaskInformation : InformationBase
    {
        public string name;

        public string icon;

        public ActionInformation action;

        public Reward reward;

        public bool main;

        public int next;

        public string description;
    }
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