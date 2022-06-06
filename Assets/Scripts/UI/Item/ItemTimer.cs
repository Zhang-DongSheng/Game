using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    public class ItemTimer : ItemBase
    {
        public UnityEvent<TimeSpan> onValueChanged;

        public UnityEvent onCompleted;

        private float timer, interval;

        private DateTime time;

        private TimeSpan span;

        private Status status;

        private void Update()
        {
            switch (status)
            {
                case Status.Update:
                    {
                        timer += Time.deltaTime;

                        if (timer > interval)
                        {
                            timer = 0;

                            OnValueChange();
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

        public void Startup(long ticks)
        {
            time =  Utility._Time.ToDateTime(ticks);

            timer = 0;

            interval = -1;

            span = time - DateTime.UtcNow;

            if (span.TotalSeconds > 0)
            {
                status = Status.Update;
            }
            else
            {
                status = Status.Complete;
            }
        }

        public void Stop()
        {
            status = Status.None;
        }

        private void OnValueChange()
        {
            span = time - DateTime.UtcNow;

            if (span.TotalSeconds > 0)
            {
                onValueChanged?.Invoke(span);

                if (span.Days > 0)
                {
                    interval = 60f * 60f;
                }
                else if (span.Hours > 0)
                {
                    interval = 60f;
                }
                else
                {
                    interval = 1f;
                }
            }
            else
            {
                status = Status.Complete;
            }
        }

        private void OnComplete()
        {
            onCompleted?.Invoke();

            status = Status.None;
        }

        enum Status
        {
            None,
            Update,
            Complete,
        }
    }
}