using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemNotice : ItemBase
    {
        public Action next;

        public Action<int> completed;

        [SerializeField] private RectTransform rect;

        [SerializeField] private Text text;

        [SerializeField] private float speed;

        [SerializeField] private int min;

        private float origin, destination, point;

        private Vector2 position;

        private NoticeStatus status;

        public int ID { get; set; }

        public float Init(string content, float screen, float space = 100)
        {
            status = NoticeStatus.First;

            text.text = content;

            float width = Mathf.Max(text.preferredWidth, min);

            origin = screen + space;

            point = screen - width;

            destination = (screen + width + space) * -1;

            this.position = new Vector2(origin, 0);

            Adapt(position, width);

            status = NoticeStatus.Next;

            return width;
        }

        public void Transition()
        {
            position += Time.deltaTime * speed * Vector2.right * -1;

            SetPosition(position);

            switch (status)
            {
                case NoticeStatus.Next:
                    if (position.x < point)
                    {
                        next?.Invoke();

                        status = NoticeStatus.Final;
                    }
                    break;
                case NoticeStatus.Final:
                    if (position.x < destination)
                    {
                        completed?.Invoke(ID);

                        status = NoticeStatus.End;
                    }
                    break;
            }
        }

        private void Adapt(Vector2 position, float width)
        {
            SetPosition(position);

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }

        public void Destroy()
        {
            if (gameObject != null)
            {
                GameObject.Destroy(gameObject);
            }
        }

        enum NoticeStatus
        {
            First,
            Next,
            Final,
            End,
        }
    }
}