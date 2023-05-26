using UnityEngine;

namespace Game.UI
{
    public class ItemAlive : ItemBase
    {
        [SerializeField] private float interval;

        private float timer;

        private bool active;

        private void OnEnable()
        {
            timer = 0;
            
            active = true;
        }

        protected override void OnUpdate(float delta)
        {
            if (active)
            {
                timer += delta;

                if (timer > interval)
                {
                    active = false;

                    SetActive(active);
                }
            }
        }

        public void StartUp()
        { 
            SetActive(true);
        }
    }
}