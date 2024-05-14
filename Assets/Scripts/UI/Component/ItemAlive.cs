using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 自动隐藏组件
    /// </summary>
    public class ItemAlive : ItemBase
    {
        [SerializeField] private float interval;

        private float timer;

        private bool active;

        protected override void OnVisible(bool active)
        {
            this.timer = 0;

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
    }
}