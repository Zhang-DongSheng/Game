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

            EventManager.Register(EventKey.Progress, Refresh);

            EventManager.Register(EventKey.UIOpen, OnUILoadingCompleted);
        }

        private void OnDestroy()
        {
            EventManager.Unregister(EventKey.Progress, Refresh);

            EventManager.Unregister(EventKey.UIOpen, OnUILoadingCompleted);
        }

        private void Refresh(EventMessageArgs args)
        {
            step = args.Get<float>("progress");

            progress.value = step;
        }

        private void OnUILoadingCompleted(EventMessageArgs args)
        {
            Close();
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