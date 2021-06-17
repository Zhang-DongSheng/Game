using UnityEngine.SAM;

namespace UnityEngine.UI
{
    /// <summary>
    /// »¬¶¯Ìõ
    /// </summary>
    [RequireComponent(typeof(SAMAction))]
    public class SliderHelper : Slider
    {
        private new SAMAction animation;

        private FloatInterval interval = new FloatInterval();

        protected override void Awake()
        {
            if (TryGetComponent(out animation))
            {
                animation.action = OnAnimation;
            }
        }

        private void OnAnimation(float value)
        {
            this.value = interval.Lerp(value);
        }

        public void Play(float target)
        {
            interval.origin = value;

            interval.destination = target;

            animation.Begin(true);
        }
    }
}