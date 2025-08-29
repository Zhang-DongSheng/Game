using Game.Data;
using Game.Network;
using Game.UI;
using Protobuf;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Logic
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

        public void RequestTasks()
        {
            var msg = new C2STaskRequest();

            NetworkManager.Instance.Send(NetworkMessageDefine.C2STaskRequest, msg, (handle) =>
            {
                _tasks.Clear();

                for (int i = 1; i < 10; i++)
                {
                    _tasks.Add(new Task()
                    {
                        identification = (uint)i,
                        parallelism = (uint)i,
                    });
                }
                ScheduleLogic.Instance.Update(Schedule.Task);
            });
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
    }
}