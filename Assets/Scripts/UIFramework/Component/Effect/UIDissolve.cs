using Game.Attribute;
using Game.UI;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Graphic))]
    public class UIDissolve : MonoBehaviour
    {
        [SerializeField] private Material source;

        [SerializeField] private List<Graphic> graphics;
        [Button("RelevanceGraphics", "自动关联")]
        [SerializeField] private bool relevance;

        private Material material;

        private float interval;

        private float timer;

        private float progress;

        private Fade state;

        public UnityEvent<Fade> onComplete;

        private void Awake()
        {
            material = new Material(source);

            for (int i = 0; i < graphics.Count; i++)
            {
                graphics[i].material = material;
            }
        }

        private void Update()
        {
            var delta = Time.deltaTime;

            switch (state)
            {
                case Fade.In:
                    {
                        timer += delta;

                        progress = timer / interval;

                        OnValueChanged(progress);

                        if (timer > interval)
                        {
                            OnComplete(state);
                        }
                    }
                    break;
                case Fade.Out:
                    {
                        timer += delta;

                        progress = 1 - timer / interval;

                        OnValueChanged(progress);

                        if (timer > interval)
                        {
                            OnComplete(state);
                        }
                    }
                    break;
            }
        }

        private void OnValueChanged(float value)
        {
            material.SetFloat("_DissolveThreshold", value);
        }

        private void OnComplete(Fade state)
        {
            this.state = Fade.None;

            onComplete?.Invoke(state);
        }
        // 编辑器方法引用
        protected void RelevanceGraphics()
        {
            graphics.Clear();

            graphics.AddRange(GetComponentsInChildren<Graphic>());
        }

        public void Startup(float interval, bool forward)
        {
            this.timer = 0;

            this.interval = Mathf.Max(0.01f, interval);

            this.state = forward ? Fade.In : Fade.Out;
        }
    }
}