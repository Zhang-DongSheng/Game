using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Animation
{
    /// <summary>
    /// 行动队列
    /// </summary>
    public class ActionQueue : ItemBase
    {
        public enum State
        {
            Create,
            Play,
            Revert,
        }
        [SerializeField] private float interval = 0.1f;

        [SerializeField] private bool active = true;

        public UnityEvent<GameObject, State> callback;

        private float timer;

        private readonly List<GameObject> stack = new List<GameObject>();

        protected override void OnUpdate(float delta)
        {
            if (!active) return;

            timer += delta;

            if (timer > interval)
            {
                timer = 0;

                Play();
            }
        }

        public void OnValueChanged(GameObject item, int volume = -1)
        {
            if (volume > 0)
            {
                if (stack.Count >= volume)
                {
                    callback?.Invoke(stack[0], State.Revert);

                    stack.RemoveAt(0);
                }
                stack.Add(item);
            }
            else
            {
                stack.Add(item);
            }
            callback?.Invoke(item, State.Create);
        }

        private void Play()
        {
            if (stack.Count > 0)
            {
                callback?.Invoke(stack[0], State.Play);

                stack.RemoveAt(0);
            }
        }
    }
}