using Game.Data;
using Game.Logic;
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
            if (NotificationLogic.Instance.Complete(NotificationType.Notice)) return;

            timer = 0;

            var notice = NotificationLogic.Instance.Pop(NotificationType.Notice);

            var item = items.Find(x => !x.gameObject.activeSelf);

            if (item == null)
            {
                item = prefab.Create<ItemNotice>();

                items.Add(item);
            }
            item.Refresh(notice.content);
        }
    }
}