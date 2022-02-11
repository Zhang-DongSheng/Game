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

        private void Update()
        {
            switch (status)
            {
                case Status.Toast:
                    {
                        timer += Time.deltaTime;

                        position += Vector3.up * _speed * Time.deltaTime;

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

            SetActive(true);

            status = Status.Toast;
        }

        private void OnComplete()
        {
            onCompleted?.Invoke();

            SetActive(false);

            status = Status.None;
        }

        enum Status
        {
            None,
            Toast,
            Complete
        }
    }
}