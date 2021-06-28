using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ScheduleLogic : Singleton<ScheduleLogic>, ILogic
    {
        private readonly List<ScheduleData> schedule = new List<ScheduleData>();

        public float progress { get; private set; }

        public void Init() { }

        public void Ready()
        {
            Add(Schedule.Config);
            Add(Schedule.Language);
            Add(Schedule.BlockedText);
        }

        public void Enter()
        {
            Add(Schedule.Shop);

            User();

            Today();
        }

        public void User()
        {
            Add(Schedule.Bag);
            Add(Schedule.Mail);
            Add(Schedule.Reddot);
        }

        public void Today()
        {
            Add(Schedule.Activity);
        }

        public void Begin()
        {
            Update(Schedule.Count);
        }

        public void Add(Schedule key)
        {
            if (schedule.Exists(a => a.key == key)) return;

            schedule.Add(new ScheduleData() { key = key, status = ScheduleStatus.Idle });
        }

        public void Update(Schedule key, ScheduleStatus status = ScheduleStatus.Complete)
        {
            for (int i = 0; i < schedule.Count; i++)
            {
                if (schedule[i].key == key)
                {
                    schedule[i].status = status;
                    break;
                }
            }
            Notice();

            if (schedule.Exists(x => x.status == ScheduleStatus.Execute)) return;

            foreach (var sc in schedule)
            {
                if (sc.status == ScheduleStatus.Idle)
                {
                    Next(sc.key);
                    break;
                }
            }
        }

        private void Next(Schedule key)
        {
            for (int i = 0; i < schedule.Count; i++)
            {
                if (schedule[i].key == key)
                {
                    schedule[i].status = ScheduleStatus.Execute;
                    break;
                }
            }

            switch (key)
            {
                default:
                    Debug.LogErrorFormat("Can't has the schedule : [{0}]", key);
                    break;
            }
        }

        private void Notice()
        {
            int number = 0;

            foreach (var sc in schedule)
            {
                if (sc.status == ScheduleStatus.Complete)
                {
                    number++;
                }
            }
            int count = Mathf.Max(schedule.Count, 1);

            progress = number / (float)count;

            if (number == schedule.Count)
            {
                schedule.Clear();
            }
        }
    }


    public class ScheduleData
    {
        public Schedule key;

        public ScheduleStatus status;
    }

    public enum ScheduleStatus
    {
        Idle,
        Execute,
        Complete,
    }

    public enum Schedule
    {
        /// <summary>
        /// ����
        /// </summary>
        Config,
        /// <summary>
        /// ���԰�
        /// </summary>
        Language,
        /// <summary>
        /// ������
        /// </summary>
        BlockedText,
        /// <summary>
        /// �̵�
        /// </summary>
        Shop,
        /// <summary>
        /// ���䡾������ء�
        /// </summary>
        Mail,
        /// <summary>
        /// ������������ء�
        /// </summary>
        Bag,
        /// <summary>
        /// ��㡾������ء�
        /// </summary>
        Reddot,
        /// <summary>
        /// �
        /// </summary>
        Activity,
        /// <summary>
        /// ����
        /// </summary>
        Count,
    }
}