using Game;
using System.Collections.Generic;

namespace Game.Data
{
    public class DataTask : DataBase
    {
        public List<TaskInformation> list = new List<TaskInformation>();

        public static TaskInformation Get(uint taskID)
        {
            var data = DataManager.Instance.Load<DataTask>();

            if (data != null)
            {
                return DataManager.Get(data.list, taskID, data.order);
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

                list.Add(task);
            }
        }

        public override void Sort()
        {
            list.Sort(InformationBase.Compare);

            order = true;
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