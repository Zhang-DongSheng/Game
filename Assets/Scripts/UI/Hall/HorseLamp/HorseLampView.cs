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
        [SerializeField] private PrefabTemplateBehaviour prefab;

        [SerializeField] private SMSize show;

        [SerializeField] private SMSize hide;

        private State state = State.Idle;

        private float timer = 0;

        private float next, complete;

        private readonly List<ItemHorseLamp> items = new List<ItemHorseLamp>();

        public override void Refresh(UIParameter paramter)
        {
            string value = paramter.Get<string>("message");

            NotificationLogic.Instance.Push(Notification.HorseLamp, value);

            switch (state)
            {
                case State.Idle:
                    {
                        show.Begin();
                    }
                    break;
                case State.Complete:
                    {

                    }
                    break;
            }
        }

        protected override void OnAwake()
        {
            show.onCompleted.AddListener(() =>
            {
                state = State.Idle; Execute();
            });
            hide.onCompleted.AddListener(() =>
            {
                state = State.Idle; OnClickClose();
            });
        }

        protected override void OnVisible(bool active)
        {
            if (active)
            {
                show.Begin();
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
            if (NotificationLogic.Instance.Empty(Notification.HorseLamp)) return;

            string content = NotificationLogic.Instance.Pop(Notification.HorseLamp);

            var item = items.Find(x => !x.isActiveAndEnabled);

            if (item == null)
            {
                item = prefab.Create<ItemHorseLamp>();
            }
            item.Refresh(content);

            complete = item.Duration;

            next = item.Next;

            timer = 0;

            state = State.Display;
        }

        private void Complete()
        {
            state = State.Complete;

            hide.Begin();
        }

        enum State
        { 
            Idle,
            Display,
            Complete,
        }
    }
}