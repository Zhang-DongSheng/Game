using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animation))]
    public class AnimationListener : MonoBehaviour
    {
        [SerializeField] private Animation _animation;

        private AnimationClip _clip;

        private Status _status;

        private float _interval;

        private readonly Dictionary<string, Action> _listeners = new Dictionary<string, Action>();

        private void Awake()
        {
            if (_animation == null)
                _animation = GetComponent<Animation>();
            _animation.playAutomatically = false;
        }

        private void Update()
        {
            switch (_status)
            {
                case Status.Playing:
                    {
                        if (Time.time > _interval)
                        {
                            _status = Status.Complete;
                        }
                    }
                    break;
                case Status.Complete:
                    {
                        Complete();
                    }
                    break;
            }
        }

        private void Execute(Action callback)
        {
            _interval = Time.time + _clip.length;

            _listeners[_clip.name] = callback;

            if (_animation.isPlaying)
            {
                _animation.Stop();
            }
            _animation.Play();

            _status = Status.Playing;
        }

        private void Complete()
        {
            if (_listeners.TryGetValue(_clip.name, out Action action))
            {
                action?.Invoke();
            }
            _status = Status.Idle;
        }

        public void Play(Action callback)
        {
            _clip = _animation.clip;

            if (_clip == null) return;

            Execute(callback);
        }

        public void Play(string name, Action callback)
        {
            var state = _animation[name];

            _clip = state != null ? state.clip : null;

            if (_clip == null) return;

            Execute(callback);
        }

        public void Stop()
        {
            if (_animation.isPlaying)
            {
                _animation.Stop();
            }
            _status = Status.Complete;
        }

        enum Status
        {
            Idle,
            Playing,
            Pause,
            Complete,
        }
    }
}