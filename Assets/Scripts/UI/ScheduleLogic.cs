using Game.Resource;
using Game.UI;
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
                        HotfixLogic.Instance.Detection();
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

                UnityEngine.EventArgs args = new UnityEngine.EventArgs();

                args.AddOrReplace("progress", progress);

                EventDispatcher.Post(UIEvent.Progress, args);
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
        /// ���޸�
        /// </summary>
        Hotfix,
        /// <summary>
        /// ���԰�
        /// </summary>
        Language,
        /// <summary>
        /// ������
        /// </summary>
        BlockedText,
        /// <summary>
        /// ����
        /// </summary>
        Mail,
        /// <summary>
        /// ����
        /// </summary>
        Bag,
        /// <summary>
        /// ����
        /// </summary>
        Chat,
        /// <summary>
        /// ���ֲ�
        /// </summary>
        Club,
        /// <summary>
        /// ����
        /// </summary>
        Friend,
        /// <summary>
        /// �̵�
        /// </summary>
        Shop,
        /// <summary>
        /// �
        /// </summary>
        Activity,
        /// <summary>
        /// ���а�
        /// </summary>
        RankingList,
        /// <summary>
        /// ����
        /// </summary>
        Task,
        /// <summary>
        /// ����
        /// </summary>
        Guidance,
        /// <summary>
        /// ����
        /// </summary>
        Count,
    }
}