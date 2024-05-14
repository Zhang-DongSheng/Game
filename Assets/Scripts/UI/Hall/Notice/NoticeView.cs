using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class NoticeView : ViewBase
    {
        [SerializeField, Range(0, 10)] private float interval = 3;

        [SerializeField] private PrefabTemplateBehaviour prefab;

        private float timer;

        private readonly List<ItemNotice> items = new List<ItemNotice>();

        public override void Refresh(UIParameter paramter)
        {
            string value = paramter.Get<string>("notice");

            NotificationLogic.Instance.Push(Notification.Notice, value);
        }

        protected override void OnAwake()
        {
            timer = interval;
        }

        protected override void OnUpdate(float delta)
        {
            timer += delta;

            if (timer > interval)
            {
                Execute();
            }
        }

        private void Execute()
        {
            if (NotificationLogic.Instance.Empty(Notification.Notice)) return;

            timer = 0;

            string content = NotificationLogic.Instance.Pop(Notification.Notice);

            var item = items.Find(x => !x.isActiveAndEnabled);

            if (item == null)
            {
                item = prefab.Create<ItemNotice>();

                items.Add(item);
            }
            item.Refresh(content);
        }
    }
}