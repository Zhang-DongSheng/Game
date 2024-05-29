using Data;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class TaskLogic : Singleton<TaskLogic>, ILogic
    {
        private readonly List<Task> _tasks = new List<Task>();

        public void Initialize()
        {

        }

        public void RequestTasks()
        {
            ScheduleLogic.Instance.Update(Schedule.Task);
        }

        public void RequestGetTaskRewards(uint taskID)
        {
            var task = _tasks.Find(x => x.parallelism == taskID);

            if (task != null)
            {
                task.status = Status.Claimed;
            }
            EventDispatcher.Post(UIEvent.Task);
        }

        public static void Goto(int type)
        {
            Debuger.Log(Author.UI, "To:" + type);
        }

        public List<Task> Tasks
        {
            get { return _tasks; }
        }
    }
}