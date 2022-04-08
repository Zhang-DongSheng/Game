using System;

namespace UnityEngine.UI
{
    public abstract class Tab : MonoBehaviour
    {
        [SerializeField] protected Button button;

        public Action<int, int> callback;

        protected bool select;

        protected int index, value;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Initialize(int index, Action<int, int> action)
        {
            this.index = index; callback = action;
        }

        public void Refresh(int value)
        {
            this.value = value;
        }

        public void Select(int index)
        {
            bool _select = this.index.Equals(index);

            if (select != _select)
            {
                select = _select;

                Switch(select);
            }
        }

        public void SetActive(bool active)
        {
            SetActive(gameObject, active);
        }

        protected virtual void OnClick()
        {
            callback?.Invoke(index, value);
        }

        protected virtual void Switch(bool state)
        {

        }

        protected virtual void SetActive(GameObject go, bool active)
        {
            if (go != null && go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }
    }
}