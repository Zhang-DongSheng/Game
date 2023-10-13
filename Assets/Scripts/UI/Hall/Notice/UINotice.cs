using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UINotice : UIBase
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

        public override void Refresh(UIParameter paramter)
        {
            string value = paramter.Get<string>("notice");

            NoticeLogic.Instance.Push(value);
        }

        private void Execute()
        {
            if (NoticeLogic.Instance.Empty) return;

            timer = 0;

            string content = NoticeLogic.Instance.Pop();

            var item = items.Find(x => x.Active == false);

            if (item != null)
            {
                item.Startup(content);
            }
            else
            {
                item = prefab.Create<ItemNotice>();

                item.Startup(content);

                items.Add(item);
            }
        }
    }
}