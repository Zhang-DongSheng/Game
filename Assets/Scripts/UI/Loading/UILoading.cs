using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UILoading : MonoSingleton<UILoading>
    {
        [SerializeField] private Slider progress;

        private float step;

        private void Awake()
        {
            progress.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnEnable()
        {
            EventDispatcher.Register(UIEvent.Progress, Refresh);
        }

        private void OnDisable()
        {
            EventDispatcher.Unregister(UIEvent.Progress, Refresh);
        }

        private void Refresh(EventArgs args)
        {
            step = args.Get<float>("progress");

            progress.value = step;
        }

        private void OnValueChanged(float value)
        {

        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}