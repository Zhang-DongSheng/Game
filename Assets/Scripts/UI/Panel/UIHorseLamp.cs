using System.Collections.Generic;
using UnityEngine;
using Game.SM;

namespace Game.UI
{
    /// <summary>
    /// ≈‹¬Ìµ∆
    /// </summary>
    public class UIHorseLamp : UIBase
    {
        [SerializeField] private PrefabWithParent prefab;

        [SerializeField] private SMSize animator;

        private NoticeStatus status;

        private int nextID;

        private readonly List<string> message = new List<string>();

        private readonly List<ItemHorseLamp> items = new List<ItemHorseLamp>();

        protected override void OnEnable()
        {
            base.OnEnable();
            animator.onCompleted.RemoveAllListeners();
            animator.Begin();
        }

        protected override void OnUpdate(float delta)
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

        public override void Refresh(Paramter paramter)
        {
            if (paramter == null) return;

            message.Add(paramter.Get<string>("message"));

            if (status == NoticeStatus.None)
            {
                status = NoticeStatus.Compute;
            }
        }

        private void Compute()
        {
            if (message.Count > 0)
            {
                ItemHorseLamp item = prefab.Create<ItemHorseLamp>();

                item.ID = NextID;

                item.next = Next;

                item.completed = Completed;

                item.Init(message[0], Config.ScreenHalfWidth);

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
                OnClickClose();
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