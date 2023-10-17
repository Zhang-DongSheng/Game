using UnityEngine;

namespace Game.UI
{
    public class ItemAlive : ItemBase
    {
        [SerializeField] private float interval;

        private float timer;

        private bool active;

        protected override void OnVisible(bool active)
        {
            timer = 0;

            this.active = active;
        }

        protected override void OnUpdate(float delta)
        {
            if (active)
            {
                timer += delta;

                if (timer > interval)
                {
                    SetActive(false);
                }
            }
        }

        public void StartUp()
        {
            SetActive(true);
        }
    }
}