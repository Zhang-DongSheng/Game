using System;

namespace UnityEngine.UI
{
    [Serializable]
    public class ScrollAlign
    {
        [SerializeField] private float ratio = 1f;

        public Action<Vector2> onValueChanged;

        public Action onCompleted;

        private Vector2 origination;

        private Vector2 destination;

        private Vector2 next;

        private Vector2 vector;

        private float step;

        private bool align;

        public void StartUp(Vector2 origination, Vector2 destination)
        {
            this.origination = origination;

            this.destination = destination;

            step = 0; align = true;
        }

        public void Update()
        {
            if (align)
            {
                step += Time.deltaTime * ratio;

                next = Vector2.Lerp(origination, destination, step);

                vector = next - origination;

                origination = next;

                onValueChanged?.Invoke(vector);

                if (step > 1)
                {
                    align = false; onCompleted?.Invoke();
                }
            }
        }
    }
}