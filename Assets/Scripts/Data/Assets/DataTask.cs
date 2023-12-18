using Game;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Data
{
    public class DataTask : DataBase
    {
        public List<TaskInformation> tasks = new List<TaskInformation>();

        public TaskInformation Get(uint key)
        {
            return tasks.Find(x => x.primary == key);
        }

        public override void Set(string content)
        {
            base.Set(content);

            int count = m_list.Count;

            for (int i = 0; i < count; i++)
            {
                var task = m_list[i].GetType<TaskInformation>();

                task.primary = m_list[i].GetUInt("ID");

                task.rewards = new List<RewardInformation>();

                var rewards = m_list[i].GetJson("rewards");

                for (int j = 0; j < rewards.Count; j++)
                {
                    task.rewards.Add(new RewardInformation()
                    {
                        propID = uint.Parse(rewards[j][0].ToString()),
                        amount = int.Parse(rewards[j][1].ToString()),
                    });
                }
                tasks.Add(task);
            }
        }

        public override void Clear()
        {
            tasks = new List<TaskInformation>();
        }
    }
    [System.Serializable]
    public class TaskInformation : InformationBase
    {
        public string name;

        public int type;

        public string icon;

        public float[] parameter;

        public List<RewardInformation> rewards;

        public string description;
    }
}