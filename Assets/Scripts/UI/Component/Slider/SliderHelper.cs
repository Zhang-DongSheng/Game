using Game.SAM;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Slider), typeof(SAMImage))]
    public class SliderHelper : MonoBehaviour
    {
        [SerializeField] protected Slider slider;

        [SerializeField] protected SAMImage sam;

        private float before;

        public void SetValue(float value)
        {
            if (slider.value != value)
            {
                before = slider.value;

                slider.value = value;

                sam?.Begin(Progress(before), Progress(value));
            }
        }

        private float Progress(float value)
        {
            return (value - slider.minValue) / (slider.maxValue - slider.minValue);
        }
    }
}