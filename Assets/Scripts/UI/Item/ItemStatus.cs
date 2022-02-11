using Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemStatus : ItemBase
    {
        [SerializeField] private GameObject _undone;

        [SerializeField] private GameObject _available;

        [SerializeField] private GameObject _received;

        public UnityEvent onUndone, onAvailable, onReceived;

        private void Awake()
        {
            if (_undone.TryGetComponent(out Button undone))
            {
                undone.onClick.AddListener(OnClickUndone);
            }

            if (_available.TryGetComponent(out Button available))
            {
                available.onClick.AddListener(OnClickAvailable);
            }

            if (_received.TryGetComponent(out Button received))
            {
                received.onClick.AddListener(OnClickReceived);
            }
        }

        public void Refresh(TaskStatus status)
        {
            SetActive(_undone, false);

            SetActive(_available, false);

            SetActive(_received, false);

            switch (status)
            {
                case TaskStatus.Undone:
                    SetActive(_undone, true);
                    break;
                case TaskStatus.Available:
                    SetActive(_available, true);
                    break;
                case TaskStatus.Received:
                    SetActive(_received, true);
                    break;
            }
        }

        private void OnClickUndone()
        {
            onUndone?.Invoke();
        }

        private void OnClickAvailable()
        {
            onAvailable?.Invoke();
        }

        private void OnClickReceived()
        {
            onReceived?.Invoke();
        }
    }
}