using Game.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ScheduleLogic : Singleton<ScheduleLogic>
    {
        private readonly List<ScheduleData> schedule = new List<ScheduleData>();

        private float progress;

        public Action callback;

        public void Initialize()
        {
            Add(Schedule.Config);
            Add(Schedule.Resource);
            Add(Schedule.Hotfix);
            Add(Schedule.Language);
            Add(Schedule.BlockedText);
            Begin();
        }

        public void Enter()
        {
            Add(Schedule.Bag);
            Add(Schedule.Mail);
            Add(Schedule.Reddot);
            Add(Schedule.Shop);
            Add(Schedule.Task);
            Add(Schedule.Activity);
            Begin();
        }

        private void Begin()
        {
            Update(Schedule.Count);
        }

        private void Add(Schedule key)
        {
            if (schedule.Exists(a => a.key == key)) return;

            schedule.Add(new ScheduleData() { key = key, status = ScheduleStatus.Ready });
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
                if (sc.status == ScheduleStatus.Ready)
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
                case Schedule.Config:
                    Update(Schedule.Config);
                    break;
                case Schedule.Resource:
                    ResourceUpdate.Instance.Direction(ResourceConfig.Loading);
                    break;
                case Schedule.Hotfix:
                    {
                        IFixLogic.Instance.Initialize();

                        ILRuntimeLogic.Instance.Initialize();
                    }
                    break;
                case Schedule.Language:
                    LanguageManager.Instance.Initialize();
                    break;
                case Schedule.BlockedText:
                    Update(Schedule.BlockedText);
                    break;
                case Schedule.Bag:
                    WarehouseLogic.Instance.RequestInformation();
                    break;
                case Schedule.Mail:
                    MailLogic.Instance.RequestMails();
                    break;
                case Schedule.Reddot:
                    ReddotLogic.Instance.RequestInformation();
                    break;
                case Schedule.Shop:
                    ShopLogic.Instance.RequestInformation();
                    break;
                case Schedule.Activity:
                    ActivityLogic.Instance.RequestInformation();
                    break;
                case Schedule.Task:
                    TaskLogic.Instance.RequestTasks();
                    break;
                case Schedule.Count:
                    Update(Schedule.Count, ScheduleStatus.Complete);
                    break;
                default:
                    Debuger.LogError(Author.Script, string.Format("must deal with the schedule [{0}]!", key));
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
            int count = schedule.Count;

            if (count > 0)
            {
                progress = number / (float)count;

                EventMessageArgs args = new EventMessageArgs();

                args.AddOrReplace("progress", progress);

                EventManager.Post(EventKey.Progress, args);
            }

            if (number == count)
            {
                schedule.Clear(); callback?.Invoke();
            }
            Debuger.Log(Author.Resource, $"��ǰ����{number}/{count}");
        }
    }


    public class ScheduleData
    {
        public Schedule key;

        public float weight;

        public float progress;

        public ScheduleStatus status;
    }

    public enum ScheduleStatus
    {
        Ready,
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
        /// ��Դ
        /// </summary>
        Resource,
        /// <summary>
        /// ���԰�
        /// </summary>
        Language,
        /// <summary>
        /// ���޸�
        /// </summary>
        Hotfix,
        /// <summary>
        /// ������
        /// </summary>
        BlockedText,
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
        /// �̵�
        /// </summary>
        Shop,
        /// <summary>
        /// �
        /// </summary>
        Activity,
        /// <summary>
        /// ����
        /// </summary>
        Task,
        /// <summary>
        /// ����
        /// </summary>
        Count,
    }
}