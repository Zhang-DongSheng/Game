using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemToast : ItemBase
    {
        [SerializeField] private Transform _target;

        [SerializeField] private Text _message;

        [SerializeField] private Vector3 _position;

        [SerializeField] private float _interval;

        [SerializeField] private float _speed;

        public UnityEvent onCompleted;

        private Vector3 position;

        private float timer;

        private Status status;

        protected override void OnUpdate(float delta)
        {
            switch (status)
            {
                case Status.Toast:
                    {
                        timer += delta;

                        position += Vector3.up * _speed * delta;

                        _target.localPosition = position;

                        if (timer > _interval)
                        {
                            status = Status.Complete;
                        }
                    }
                    break;
                case Status.Complete:
                    {
                        OnComplete();
                    }
                    break;
            }
        }

        public void Startup(string message)
        {
            position = _position;

            _target.localPosition = position;

            _message.text = message;

            status = Status.Toast;

            SetActive(true);
        }

        private void OnComplete()
        {
            onCompleted?.Invoke();

            status = Status.None;

            SetActive(false);
        }

        enum Status
        {
            None,
            Toast,
            Complete
        }
    }
}