using Game.UI;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public class LayoutAdaptiveSize : MonoBehaviour
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private Transform group;

        [SerializeField] private Vector2 space;

        [SerializeField] private Axis axis;

        private readonly List<RectTransform> children = new List<RectTransform>();

        private void Awake()
        {
            if (group == null) return;

            int count = group.childCount;

            for (int i = 0; i < count; i++)
            {
                children.Add(group.GetChild(i).GetComponent<RectTransform>());
            }
        }

        private void OnEnable()
        {
            Refresh();
        }

        public void Refresh()
        {
            StartCoroutine(Format());
        }

        private IEnumerator Format()
        {
            yield return new WaitForEndOfFrame();

            switch (axis)
            {
                case Axis.Horizontal:
                    {
                        Horizontal();
                    }
                    break;
                case Axis.Vertical:
                    {
                        Vertical();
                    }
                    break;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(target);

            var parent = GetComponentInParent<ContentSizeFitter>();

            if (parent != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
            }
        }

        private void Horizontal()
        {
            float value = 0;

            int count = children.Count;

            for (int i = 0; i < count; i++)
            {
                if (children[i].TryGetComponent(out Text text))
                {
                    value += text.preferredWidth;
                }
                else
                {
                    value += children[i].rect.width;
                }

                if (i + 1 < count)
                {
                    value += space.x;
                }
            }
            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
        }

        private void Vertical()
        {
            float value = 0;

            int count = children.Count;

            for (int i = 0; i < count; i++)
            {
                if (children[i].TryGetComponent(out Text text))
                {
                    value += text.preferredHeight;
                }
                else
                {
                    value += children[i].rect.height;
                }

                if (i + 1 < count)
                {
                    value += space.y;
                }
            }
            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);
        }
    }
}