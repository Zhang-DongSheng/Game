using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemHorseLamp : ItemBase
    {
        [SerializeField] private RectTransform rect;

        [SerializeField] private Text text;

        [SerializeField] private float duration = 5;

        [SerializeField] private float stay = 1;

        private float origin, destination, point;

        private Vector2 position;

        private State state;

        protected override void OnUpdate(float delta)
        {
            
        }

        public void Refresh(string content)
        {
            text.text = content;

            SetActive(true);
        }

        public float Next => duration - (duration - stay) * 0.5f;

        public float Duration => duration;

        enum State
        {
            Start,
            From,
            Stay,
            To,
            End,
        }
    }
}