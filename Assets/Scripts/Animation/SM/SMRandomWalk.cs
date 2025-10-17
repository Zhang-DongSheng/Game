using UnityEngine;

namespace Game.SM
{
    /// <summary>
    /// 随机游走
    /// </summary>
    public class SMRandomWalk : SMBase
    {
        [SerializeField] private Vector2 area;

        [SerializeField] private int type = 0;

        [SerializeField] private float ratio = 1f;

        private Vector2 vector;

        private Vector2 position;

        private bool change;

        private float distance, range;

        private RectTransform content;

        protected override void Initialize()
        {
            range = area.x * area.y;

            position = Vector2.zero;

            vector = Random.insideUnitCircle.normalized * ratio;

            content = target as RectTransform;
        }

        protected override void Transition(float progress)
        {
            content.anchoredPosition = position;
        }

        protected override void Renovate(float delta)
        {
            position += vector * delta * speed;

            change = false;

            switch (type)
            {
                case 0:
                    {
                        distance = position.sqrMagnitude;

                        if (distance > range)
                        {
                            position = position.normalized * area.x;

                            change = true;
                        }
                    }
                    break;
                case 1:
                    {
                        if (position.x.WithinTheRange(-area.x, area.x))
                        {
                            position.x = Mathf.Clamp(position.x, -area.x, area.x);

                            change = true;
                        }

                        if (position.y.WithinTheRange(-area.y, area.y))
                        {
                            position.y = Mathf.Clamp(position.y, -area.y, area.y);

                            change = true;
                        }
                    }
                    break;
            }
            Transition(progress);

            if (change)
            {
                vector = Random.insideUnitCircle.normalized * ratio;
            }
        }
    }
}