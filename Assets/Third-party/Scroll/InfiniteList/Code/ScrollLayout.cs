using System.Collections.Generic;

namespace UnityEngine.UI.Scroll
{
    [ExecuteInEditMode]
    public class ScrollLayout : MonoBehaviour
    {
        private enum Axis
        {
            Horiaontal,
            Vertical,
        }

        private enum Stretch
        {
            None,
            Horiaontal,
            Vertical,
            Full,
        }

        [SerializeField] private Axis axis = Axis.Horiaontal;
        [SerializeField] private Stretch stretch = Stretch.None;
        [SerializeField] private RectTransform anchor = null;
        [SerializeField] private Vector2 size = new Vector2(100, 100);
        [SerializeField] private Vector2 spacing = Vector2.zero;
        [SerializeField] private bool adapt = false;

        private RectTransform m_target;

        private Vector2 m_position;

        private readonly List<RectTransform> m_childs = new List<RectTransform>();

        private void Awake()
        {
            m_target = GetComponent<RectTransform>();

            UpdateStretch();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (m_target == null) return;

            UpdateStretch();

            m_childs.Clear();

            for (int i = 0; i < m_target.childCount; i++)
            {
                if (m_target.GetChild(i).gameObject.activeSelf)
                {
                    m_childs.Add(m_target.GetChild(i) as RectTransform);
                }
            }

            RefreshUI();
        }
#endif

        private void RefreshUI()
        {
            m_position = Vector2.zero;

            for (int i = 0; i < m_childs.Count; i++)
            {
                m_childs[i].anchoredPosition = m_position;

                Adapt(m_childs[i]);

                switch (axis)
                {
                    case Axis.Horiaontal:
                        m_position.x += Horizontal;
                        break;
                    case Axis.Vertical:
                        m_position.y -= Vertical;
                        break;
                }
            }
        }

        public void UpdateStretch()
        {
            switch (stretch)
            {
                case Stretch.Horiaontal:
                    size.x = anchor != null ? anchor.rect.width : m_target.rect.width;
                    break;
                case Stretch.Vertical:
                    size.y = anchor != null ? anchor.rect.height : m_target.rect.height;
                    break;
                case Stretch.Full:
                    size.x = anchor != null ? anchor.rect.width : m_target.rect.width;
                    size.y = anchor != null ? anchor.rect.height : m_target.rect.height;
                    break;
            }
        }

        public void Adapt(RectTransform rect)
        {
            if (adapt)
            {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Horizontal);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Vertical);
            }
        }

        public float Horizontal { get { return size.x + spacing.x; } }

        public float Vertical { get { return size.y + spacing.y; } }
    }
}