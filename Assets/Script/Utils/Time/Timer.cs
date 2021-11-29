namespace UnityEngine
{
    [System.Serializable]
    public class Timer
    {
        public System.Action action;

        public float interval;

        protected float timer;

        public void Update(float delta)
        {
            timer += delta;

            if (timer > interval)
            {
                timer = 0; action?.Invoke();
            }
        }
    }
}