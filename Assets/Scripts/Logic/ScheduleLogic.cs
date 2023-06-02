using Game.Resource;
using Game.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ScheduleLogic : Singleton<ScheduleLogic>, ILogic
    {
        private readonly List<ScheduleData> schedule = new List<ScheduleData>();

        private float progress;

        public Action callback;

        public void Init()
        {
            Ready();

            Begin();
        }

        public void Enter()
        {
            User();

            Today();

            Begin();
        }

        private void User()
        {
            Add(Schedule.Bag);
            Add(Schedule.Mail);
            Add(Schedule.Reddot);
        }

        private void Today()
        {
            Add(Schedule.Shop);
            Add(Schedule.Activity);
        }

        private void Ready()
        {
            Add(Schedule.Config);
            Add(Schedule.Resource);
            Add(Schedule.Language);
            Add(Schedule.BlockedText);
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
                case Schedule.Language:
                    LanguageManager.Instance.Initialize();
                    break;
                case Schedule.BlockedText:
                    Update(Schedule.BlockedText);
                    break;
                case Schedule.Bag:
                    Update(Schedule.Bag);
                    break;
                case Schedule.Mail:
                    Update(Schedule.Mail);
                    break;
                case Schedule.Reddot:
                    Update(Schedule.Reddot);
                    break;
                case Schedule.Shop:
                    Update(Schedule.Shop);
                    break;
                case Schedule.Activity:
                    Update(Schedule.Activity);
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
            Debuger.Log(Author.Resource, $"当前进度{number}/{count}");
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
        /// 配置
        /// </summary>
        Config,
        /// <summary>
        /// 资源
        /// </summary>
        Resource,
        /// <summary>
        /// 语言包
        /// </summary>
        Language,
        /// <summary>
        /// 屏蔽字
        /// </summary>
        BlockedText,
        /// <summary>
        /// 邮箱【个人相关】
        /// </summary>
        Mail,
        /// <summary>
        /// 背包【个人相关】
        /// </summary>
        Bag,
        /// <summary>
        /// 红点【个人相关】
        /// </summary>
        Reddot,
        /// <summary>
        /// 商店
        /// </summary>
        Shop,
        /// <summary>
        /// 活动
        /// </summary>
        Activity,
        /// <summary>
        /// 数量
        /// </summary>
        Count,
    }
}