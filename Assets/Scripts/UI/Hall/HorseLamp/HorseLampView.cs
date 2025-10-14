using Game.Data;
using Game.Logic;
using Game.SM;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// ≈‹¬Ìµ∆
    /// </summary>
    public class HorseLampView : ViewBase
    {
        [SerializeField] private PrefabTemplateComponent prefab;

        [SerializeField] private SMSize display;

        private State state = State.Idle;

        private float timer = 0;

        private float next, complete;

        private readonly List<ItemHorseLamp> items = new List<ItemHorseLamp>();

        protected override void OnAwake()
        {
            display.onCompleted = () =>
            {
                if (state == State.Complete)
                {
                    OnClickClose();
                }
                else
                {
                    Execute();
                }
            };
        }

        public override void Refresh(UIParameter parameter)
        {
            if (state == State.Complete)
            {
                state = State.Idle;

                display.Begin(true);
            }
        }

        protected override void OnVisible(bool active)
        {
            if (active)
            {
                display.Begin(true);
            }
        }

        protected override void OnUpdate(float delta)
        {
            timer += delta;

            switch (state)
            {
                case State.Display:
                    {
                        if (timer > complete)
                        {
                            Complete();
                        }
                        else if (timer > next)
                        {
                            Execute();
                        }
                    }
                    break;
            }
        }

        private void Execute()
        {
            if (NotificationLogic.Instance.Complete(NotificationType.HorseLamp)) return;

            var notice = NotificationLogic.Instance.Pop(NotificationType.HorseLamp);

            var item = items.Find(x => !x.gameObject.activeSelf);

            if (item == null)
            {
                item = prefab.Create<ItemHorseLamp>();

                items.Add(item);
            }
            item.Refresh(notice.content);

            complete = item.Duration;

            next = item.Next;

            timer = 0;

            state = State.Display;
        }

        private void Complete()
        {
            state = State.Complete;

            display.Begin(false);
        }

        enum State
        {
            Idle,
            Display,
            Complete,
        }
    }
}