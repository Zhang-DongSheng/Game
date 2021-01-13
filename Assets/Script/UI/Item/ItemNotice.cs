using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemNotice : ItemBase
    {
        public Action<int> callback;

        [SerializeField] private RectTransform rect;

        [SerializeField] private Text text;

        [SerializeField] private float speed;

        [SerializeField] private int min;

        private float origin, destination;

        private Vector2 position;

        public int ID { get; set; }

        public float Init(Vector2 position, string content)
        {
            text.text = content;

            origin = 1000;

            destination = -1000;

            this.position = new Vector2(origin, 0);

            float width = Mathf.Max(text.preferredWidth, min);

            Adapt(position, width);

            return width;
        }

        public void Transition()
        {
            position += Time.deltaTime * speed * Vector2.right * -1;

            target.transform.localPosition = position;

            if (position.x < destination)
            {
                callback?.Invoke(ID);
            }
        }

        private void Adapt(Vector2 position, float width)
        {
            target.transform.localPosition = position;

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }

        public void Destroy()
        {
            if (gameObject != null)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}