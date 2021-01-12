using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UINotice : UIBase
    {
        [SerializeField] private ParentAndPrefab prefab;

        [SerializeField] private float space;

        private NoticeStatus status;

        private readonly List<string> message = new List<string>();

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
                string content = message[0];

                message.RemoveAt(0);

                ItemNotice item = prefab.Create<ItemNotice>();

                item.Init(Vector2.zero, content);

                status = NoticeStatus.Transition;
            }
            else
            {
                status = NoticeStatus.Completed;
            }
        }

        private void Transition()
        {
            
        }

        private void Completed()
        {
            status = NoticeStatus.None;

            Close();
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