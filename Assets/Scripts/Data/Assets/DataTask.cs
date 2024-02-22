using Game;
using System.Collections.Generic;

namespace Data
{
    public class DataTask : DataBase
    {
        public List<TaskInformation> list = new List<TaskInformation>();

        public static TaskInformation Get(uint taskID)
        {
            var data = DataManager.Instance.Load<DataTask>();

            if (data != null)
            {
                return data.list.Find(x => x.primary == taskID);
            }
            return null;
        }

        public override void Load(string content)
        {
            base.Load(content);

            int count = m_list.Count;

            for (int i = 0; i < count; i++)
            {
                var task = m_list[i].GetType<TaskInformation>();

                task.primary = m_list[i].GetUInt("ID");

                task.rewards = new List<UIntPair>();

                var rewards = m_list[i].GetJson("rewards");

                for (int j = 0; j < rewards.Count; j++)
                {
                    task.rewards.Add(new UIntPair()
                    {
                        x = uint.Parse(rewards[j][0].ToString()),
                        y = uint.Parse(rewards[j][1].ToString()),
                    });
                }
                list.Add(task);
            }
        }

        public override void Clear()
        {
            list = new List<TaskInformation>();
        }
    }
    [System.Serializable]
    public class TaskInformation : InformationBase
    {
        public string name;

        public int type;

        public string icon;

        public float[] parameter;

        public List<UIntPair> rewards;

        public string description;
    }
}