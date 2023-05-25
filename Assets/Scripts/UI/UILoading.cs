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
            EventManager.Register(EventKey.Progress, Refresh);
            // 打开UI后关闭界面
            EventManager.Register(EventKey.OpenPanel, Complete);
        }

        private void OnDisable()
        {
            EventManager.Unregister(EventKey.Progress, Refresh);
            // 打开UI后关闭界面
            EventManager.Unregister(EventKey.OpenPanel, Complete);
        }

        private void Refresh(EventMessageArgs args)
        {
            step = args.Get<float>("progress");

            progress.value = step;
        }

        private void Complete(EventMessageArgs args)
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