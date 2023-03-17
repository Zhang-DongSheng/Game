using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    public class ToggleGroupHelper : ToggleGroup
    {
        [SerializeField] private int index = 0;

        [SerializeField] private RectTransform cursor;

        [SerializeField, Range(1, 25f)] private float speed = 1f;

        [SerializeField] protected AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        private readonly List<Toggle> toggles = new List<Toggle>();

        [Space(5)] public UnityEvent<int> onClick;

        private float step, progress;

        private Vector2 position;

        private bool ing;

        protected override void Awake()
        {
            base.Awake();

            toggles.Clear();

            toggles.AddRange(GetComponentsInChildren<Toggle>());

            for (int i = 0; i < toggles.Count; i++)
            {
                int index = i;

                toggles[i].onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        onClick?.Invoke(index); OnValueChanged(index);
                    }
                });
            }
        }

        protected override void Start()
        {
            base.Start();

            if (toggles.Count > index)
            {
                toggles[index].isOn = true;
            }
        }

        private void Update()
        {
            if (ing)
            {
                step += Time.deltaTime * speed;

                progress = curve.Evaluate(step);

                cursor.localPosition = Vector2.Lerp(position, Vector2.zero, progress);

                if (step > 1)
                {
                    ing = false;
                }
            }
        }

        private void OnValueChanged(int index, bool animation = true)
        {
            if (cursor == null) return;

            try
            {
                if (toggles[index].TryGetComponent(out ToggleHelper helper))
                {
                    cursor.SetParent(helper.Node);

                    if (animation)
                    {
                        position = cursor.localPosition;

                        step = 0; ing = true;
                    }
                    else
                    {
                        position = Vector2.zero;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public void DesignatedIndex(int index)
        {
            this.index = index;
        }
    }
}