using System.Collections.Generic;

namespace Data
{
    public class DataTask : DataBase
    {
        public List<TaskInformation> tasks = new List<TaskInformation>();

        public TaskInformation Get(int identification, bool quick = false)
        {
            if (quick)
            {
                return QuickLook(tasks, identification);
            }
            else
            {
                return tasks.Find(x => x.identification == identification);
            }
        }

        protected override void Editor()
        {
            tasks.Sort((a, b) =>
            {
                return a.identification > b.identification ? 1 : -1;
            });
        }
    }
    [System.Serializable]
    public class TaskInformation : InformationBase
    {
        public string name;

        public string icon;

        public int next;

        public int count;

        public string description;

        public Reward reward;
    }
}