using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SAM;
using UnityEngine.UI;

namespace Game.UI
{
    public class UINotice : UIBase
    {
        [SerializeField] private ParentAndPrefab prefab;

        [SerializeField] private float space;

        [SerializeField] private SAMSize animator;

        private NoticeStatus status;

        private int nextID;

        private readonly List<string> message = new List<string>();

        private readonly List<ItemNotice> items = new List<ItemNotice>();

        private void OnEnable()
        {
            animator.onCompleted.RemoveAllListeners();
            animator.Begin(true);
        }

        private void Update()
        {
            switch (status)
            {
                case NoticeStatus.Compute:
                    Compute();
                    break;
                case NoticeStatus.Transition:
                    Transition();
                    break;
                case NoticeStatus.Completed:
                    Completed();
                    break;
            }
        }

        public override void Refresh(params object[] paramter)
        {
            if (paramter == null) return;

            message.Add(paramter[0].ToString());

            if (status == NoticeStatus.None)
            {
                status = NoticeStatus.Compute;
            }
        }

        private void Compute()
        {
            if (message.Count > 0)
            {
                ItemNotice item = prefab.Create<ItemNotice>();

                item.ID = NextID;

                item.next = Next;

                item.completed = Completed;

                item.Init(message[0], UIConfig.ScreenHalfWidth);

                items.Add(item);

                message.RemoveAt(0);

                status = NoticeStatus.Transition;
            }
        }

        private void Next()
        {
            Compute();
        }

        private void Completed(int ID)
        {
            int index = items.FindIndex(x => x.ID == ID);

            if (index != -1)
            {
                items[index].Destroy();
                items.RemoveAt(index);
            }

            if (items.Count == 0)
            {
                status = NoticeStatus.Completed;
            }
        }

        private void Transition()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Transition();
            }
        }

        private void Completed()
        {
            status = NoticeStatus.None;

            animator.onCompleted.AddListener(() =>
            {
                UIManager.Instance.Close(UIPanel.UINotice);
            });
            animator.Begin(false);
        }

        private int NextID
        {
            get { return nextID++; }
        }
    }

    enum NoticeStatus
    {
        None,
        Compute,
        Transition,
        Completed,
    }
}