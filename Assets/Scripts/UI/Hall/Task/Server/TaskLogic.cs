using Data;
using System.Collections.Generic;

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
            int count = 9;

            for (int i = 0; i < count; i++)
            {
                _tasks.Add(new Task()
                {
                    identification = 10000 + (uint)i,
                    parallelism = (uint)i + 1,
                    progress = 0,
                    status = Status.Undone,
                });
            }
            ScheduleLogic.Instance.Update(Schedule.Task);
        }
    }
}