using Game.Resource;
using Game.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Logic
{
    public class ScheduleLogic : Singleton<ScheduleLogic>, ILogic
    {
        private readonly List<ScheduleStruct> schedule = new List<ScheduleStruct>();

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
            Add(Schedule.Mail);
            Add(Schedule.Bag);
            Add(Schedule.Chat);
            Add(Schedule.Club);
            Add(Schedule.Friend);
            Add(Schedule.Shop);
            Add(Schedule.Activity);
            Add(Schedule.RankingList);
            Add(Schedule.Task);
            Begin();
        }

        private void Begin()
        {
            Update(Schedule.Count);
        }

        private void Add(Schedule key)
        {
            if (schedule.Exists(a => a.key == key)) return;

            schedule.Add(new ScheduleStruct() { key = key, status = ScheduleState.None });
        }

        public void Update(Schedule key, ScheduleState status = ScheduleState.Complete)
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

            if (schedule.Exists(x => x.status == ScheduleState.Execute)) return;

            foreach (var sc in schedule)
            {
                if (sc.status == ScheduleState.None)
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
                    schedule[i].status = ScheduleState.Execute;
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
                        HotUpdateLogic.Instance.LoadILRuntime();
                    }
                    break;
                case Schedule.Language:
                    LanguageManager.Instance.Initialize();
                    break;
                case Schedule.BlockedText:
                    Update(Schedule.BlockedText);
                    break;
                case Schedule.Mail:
                    MailLogic.Instance.RequestMails();
                    break;
                case Schedule.Bag:
                    WarehouseLogic.Instance.RequestInformation();
                    break;
                case Schedule.Chat:
                    ChatLogic.Instance.RequestChat();
                    break;
                case Schedule.Club:
                    ClubLogic.Instance.RequestClub();
                    break;
                case Schedule.Friend:
                    FriendLogic.Instance.RequestFriend();
                    break;
                case Schedule.Shop:
                    ShopLogic.Instance.RequestInformation();
                    break;
                case Schedule.Activity:
                    ActivityLogic.Instance.RequestInformation();
                    break;
                case Schedule.RankingList:
                    RankingListLogic.Instance.RequestRankingList();
                    break;
                case Schedule.Task:
                    TaskLogic.Instance.RequestTasks();
                    break;
                case Schedule.Guidance:
                    GuidanceLogic.Instance.RequestGuidance();
                    break;
                case Schedule.Count:
                    Update(Schedule.Count, ScheduleState.Complete);
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
                if (sc.status == ScheduleState.Complete)
                {
                    number++;
                }
            }
            int count = schedule.Count;

            if (count > 0)
            {
                progress = number / (float)count;

                UnityEngine.EventArgs args = new UnityEngine.EventArgs();

                args.AddOrReplace("progress", progress);

                EventDispatcher.Post(UIEvent.Progress, args);
            }

            if (number == count)
            {
                schedule.Clear(); callback?.Invoke();
            }
            Debuger.Log(Author.Resource, $"当前进度{number}/{count}");
        }

        class ScheduleStruct
        {
            public Schedule key;

            public float weight;

            public float progress;

            public ScheduleState status;
        }
    }

    public enum ScheduleState
    {
        None,
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
        /// 热修复
        /// </summary>
        Hotfix,
        /// <summary>
        /// 语言包
        /// </summary>
        Language,
        /// <summary>
        /// 屏蔽字
        /// </summary>
        BlockedText,
        /// <summary>
        /// 邮箱
        /// </summary>
        Mail,
        /// <summary>
        /// 背包
        /// </summary>
        Bag,
        /// <summary>
        /// 聊天
        /// </summary>
        Chat,
        /// <summary>
        /// 俱乐部
        /// </summary>
        Club,
        /// <summary>
        /// 好友
        /// </summary>
        Friend,
        /// <summary>
        /// 商店
        /// </summary>
        Shop,
        /// <summary>
        /// 活动
        /// </summary>
        Activity,
        /// <summary>
        /// 排行榜
        /// </summary>
        RankingList,
        /// <summary>
        /// 任务
        /// </summary>
        Task,
        /// <summary>
        /// 引导
        /// </summary>
        Guidance,
        /// <summary>
        /// 数量
        /// </summary>
        Count,
    }
}