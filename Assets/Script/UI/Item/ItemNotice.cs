using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemNotice : ItemBase
    {
        [SerializeField] private RectTransform rect;

        [SerializeField] private Text text;

        [SerializeField] private int min;

        public float Init(Vector2 position, string content)
        {
            text.text = content;

            float width = Mathf.Max(text.preferredWidth, min);

            Adapt(position, width);

            return width;
        }

        private void Adapt(Vector2 position, float width)
        {
            rect.anchoredPosition = position;

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
    }
}