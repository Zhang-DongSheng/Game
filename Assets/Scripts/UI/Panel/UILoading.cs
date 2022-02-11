using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UILoading : UIBase
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
        }

        private void OnDisable()
        {
            EventManager.Unregister(EventKey.Progress, Refresh);
        }

        private void Refresh(EventMessageArgs args)
        {
            step = args.Get<float>("progress");

            progress.value = step;
        }

        private void OnValueChanged(float value)
        {

        }
    }
}