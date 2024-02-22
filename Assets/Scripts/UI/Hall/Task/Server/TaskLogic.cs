using Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TaskLogic : Singleton<TaskLogic>, ILogic
    {
        private readonly List<Task> _tasks = new List<Task>();

        public List<Task> Tasks
        {
            get { return _tasks; }
        }

        public void Initialize()
        {

        }

        public void Release()
        {

        }

        public void RequestTasks()
        {
            var data = DataManager.Instance.Load<DataTask>();

            int count = data.list.Count;

            for (int i = 0; i < count; i++)
            {
                _tasks.Add(new Task(data.list[i])
                {

                });
            }
            ScheduleLogic.Instance.Update(Schedule.Task);
        }

        public void RequestGetTaskRewards(uint taskID)
        {
            var task = _tasks.Find(x => x.parallelism == taskID);

            if (task != null)
            {
                task.status = Status.Claimed;
            }
            EventManager.Post(EventKey.Task);
        }

        public static void Goto(int type)
        {
            Debuger.Log(Author.UI, "ȥ" + type);
        }
    }
}