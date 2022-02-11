using Data;
using System.Collections.Generic;

namespace Game
{
    public class TaskLogic : Singleton<TaskLogic>, ILogic
    {
        private readonly List<Task> tasks = new List<Task>();

        private TaskInformation task;

        public void Init()
        {

        }

        public void AcceptTask(int identification)
        {
            if (tasks.Exists(x => x.identification == identification)) return;

            task = DataHelper.Task.Get(identification);

            tasks.Add(new Task()
            {
                identification = identification,
                action = task.action.type,
                progress = 0,
                main = task.main,
                status = TaskStatus.Undone,
            });
        }

        public void RenovateTask(ActionType action, float value)
        {
            int count = tasks.Count;

            for (int i = 0; i < count; i++)
            {
                if (tasks[i].status == TaskStatus.Undone &&
                    tasks[i].action == action)
                {
                    tasks[i].progress += value;

                    task = DataHelper.Task.Get(tasks[i].identification);

                    if (tasks[i].progress >= task.action.count)
                    {
                        tasks[i].status = TaskStatus.Available;
                    }
                }
            }
        }

        public void GiveUpTask(int identification)
        {
            int index = tasks.FindIndex(x => x.identification == identification);

            if (index != -1)
            {
                tasks.RemoveAt(index);
            }
        }

        public void FulfilTask(int identification)
        {
            int index = tasks.FindIndex(x => x.identification == identification);

            if (index != -1)
            {
                if (tasks[index].status == TaskStatus.Received)
                {
                    tasks.RemoveAt(index);
                }
            }
        }
    }
}