using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI.Scroll;

namespace UnityEngine.UI
{
    public class ScrollList : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Action<int> callBack;
        [SerializeField]
        private Direction direction;
        [SerializeField]
        private Transform content;
        [SerializeField, Range(0.1f, 5), Tooltip("拖拽速度")]
        private float speed = 1;
        [SerializeField, Range(1, 20f), Tooltip("居中速度")]
        private float ratio = 1;
        [SerializeField, Range(10, 1000), Tooltip("滑动力度 ")]
        private float spring = 30;
        [SerializeField, Range(1, 60), Tooltip("显示间隔(s)")]
        private float interval = 3;
        [SerializeField, Tooltip("自动显示")]
        private bool auto;
        [SerializeField, Tooltip("多个滑动组件")]
        private bool multi;
        [SerializeField]
        private Vector2 center;

        private Vector2 space;

        private Vector2 cell;

        private Vector2 vector;

        private float offset;

        private Vector2 position;

        private Vector2 point_begin;

        private Vector2 point_end;

        private int index_pre;

        private int index_target;

        private float spring_value;

        private int spring_offset;

        private bool center_state;

        private float center_step;

        private Vector2 center_vector;

        private Vector2 center_position;

        private float auto_timer;

        private bool onlyOne;

        private ScrollRect multiScroll;

        private ScrollCtrl ctrl;

        private readonly List<RectTransform> m_childs = new List<RectTransform>();

        private void Awake()
        {
            if (multiScroll == null)
                multiScroll = GetComponentInParent<ScrollRect>();
            FormatPosition();
        }

        private void Update()
        {
            if (!Drag && center_state)
            {
                center_step += Time.deltaTime * ratio;
                center_position = Vector2.Lerp(m_childs[index_target].localPosition, center, center_step);
                center_vector = center_position - (Vector2)m_childs[index_target].localPosition;

                Move(center_vector);

                if (center_step > 1)
                {
                    center_state = false;
                }
            }

            if (Auto)
            {
                if (Drag)
                    auto_timer = 0;
                else
                    auto_timer += Time.deltaTime;

                if (auto_timer >= interval)
                {
                    auto_timer = 0;

                    Next();
                }
            }
        }

        public bool Auto { get { return auto; } set { auto = value; } }

        public bool Lock { get; set; }

        public void OnBeginDrag(PointerEventData eventData)
        {
            switch (direction)
            {
                case Direction.Horizontal:
                    ctrl = ScrollUtils.Horizontal(eventData.delta) ? ScrollCtrl.Single : ScrollCtrl.Multi;
                    break;
                case Direction.Vertical:
                    ctrl = ScrollUtils.Vertical(eventData.delta) ? ScrollCtrl.Single : ScrollCtrl.Multi;
                    break;
            }

            if (multi && ctrl == ScrollCtrl.Multi && multiScroll != null)
            {
                multiScroll.OnBeginDrag(eventData);
            }
            else
            {
                OnBeginDrag();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (multi && ctrl == ScrollCtrl.Multi && multiScroll != null)
            {
                multiScroll.OnDrag(eventData);
            }
            else
            {
                OnDrag(eventData.delta);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (multi && ctrl == ScrollCtrl.Multi && multiScroll != null)
            {
                multiScroll.OnEndDrag(eventData);
            }
            else
            {
                OnEndDrag();
            }
        }

        public void FormatPosition()
        {
            m_childs.Clear();

            ScrollLayout _content = content.GetComponent<ScrollLayout>();

            if (_content == null)
                Debug.LogError("Please AddCompontent<Content> on 'Content'");

            cell.x = _content.Horizontal;
            cell.y = _content.Vertical;

            for (int i = 0; i < content.childCount; i++)
            {
                if (content.GetChild(i).gameObject.activeSelf)
                {
                    m_childs.Add(content.GetChild(i) as RectTransform);
                }
            }

            onlyOne = m_childs.Count == 1;

            Vector2 _position = center;

            for (int i = 0; i < m_childs.Count; i++)
            {
                m_childs[i].anchorMin = Vector2.one * 0.5f;
                m_childs[i].anchorMax = Vector2.one * 0.5f;
                m_childs[i].pivot = Vector2.one * 0.5f;
                m_childs[i].anchoredPosition = _position;

                _content.Adapt(m_childs[i]);

                switch (direction)
                {
                    case Direction.Horizontal:
                        _position.x += cell.x;
                        break;
                    case Direction.Vertical:
                        _position.y -= cell.y;
                        break;
                    default:
                        _position += cell;
                        break;
                }
            }

            int preview_index = m_childs.Count > 2 ? 2 : 1;

            if (m_childs.Count > 1)
            {
                switch (direction)
                {
                    case Direction.Horizontal:
                        space.x = m_childs[0].localPosition.x - preview_index * cell.x;
                        space.y = m_childs[m_childs.Count - preview_index].localPosition.x;
                        break;
                    case Direction.Vertical:
                        space.x = m_childs[m_childs.Count - preview_index].localPosition.y;
                        space.y = m_childs[0].localPosition.y + preview_index * cell.y;
                        break;
                }
            }
            else
            {
                space = Vector2.zero;
            }
        }

        public void Jump(int index)
        {
            if (index < 0 || index >= m_childs.Count)
                return;

            Move(Vector2.one * -1);

            index_pre = index_target = index;

            callBack?.Invoke(index_target);

            center_step = 0;
            center_state = true;
        }

        private void OnBeginDrag()
        {
            if (Lock || onlyOne) return;

            point_begin = Input.mousePosition;

            Drag = true;
        }

        private void OnDrag(Vector2 vector)
        {
            if (Drag)
            {
                Move(vector * speed);
            }
        }

        private void OnEndDrag()
        {
            if (Lock || onlyOne) return; 
            
            if (!Drag) return;

            Drag = false;

            point_end = Input.mousePosition;

            index_target = -1;

            for (int i = 0; i < m_childs.Count; i++)
            {
                if (Center(m_childs[i].localPosition))
                {
                    index_target = i;
                    break;
                }
            }

            if (index_target != -1)
            {
                if (index_target == index_pre)
                {
                    switch (direction)
                    {
                        case Direction.Horizontal:
                            spring_value = point_end.x - point_begin.x;
                            break;
                        case Direction.Vertical:
                            spring_value = point_end.y - point_begin.y;
                            break;
                    }

                    if (Math.Abs(spring_value) > spring)
                    {
                        spring_offset = spring_value > 0 ? -1 : 1;

                        index_target += spring_offset;

                        if (index_target < 0)
                        {
                            index_target += m_childs.Count;
                        }
                        else if (index_target >= m_childs.Count)
                        {
                            index_target %= m_childs.Count;
                        }
                    }
                }

                index_pre = index_target;

                callBack?.Invoke(index_target);
            }

            center_step = 0;
            center_state = index_target != -1;
        }

        private void Move(Vector2 delta)
        {
            switch (direction)
            {
                case Direction.Horizontal:
                    vector.x = delta.x;
                    vector.y = 0;
                    break;
                case Direction.Vertical:
                    vector.x = 0;
                    vector.y = delta.y;
                    break;
                default:
                    vector = delta;
                    break;
            }

            for (int i = 0; i < m_childs.Count; i++)
            {
                position = m_childs[i].localPosition;
                position += vector;

                switch (direction)
                {
                    case Direction.Horizontal:
                        if (position.x > space.y)
                        {
                            offset = position.x - space.y;
                            position.x = space.x + offset;
                        }
                        else if (position.x < space.x)
                        {
                            offset = position.x - space.x;
                            position.x = space.y + offset;
                        }
                        break;
                    case Direction.Vertical:
                        if (position.y > space.y)
                        {
                            offset = position.y - space.y;
                            position.y = space.x + offset;
                        }
                        else if (position.y < space.x)
                        {
                            offset = position.y - space.x;
                            position.y = space.y + offset;
                        }
                        break;
                    default:
                        break;
                }

                m_childs[i].localPosition = position;
            }
        }

        private void Next()
        {
            if (m_childs.Count == 0) return;

            index_target++;

            if (index_target >= m_childs.Count)
            {
                index_target %= m_childs.Count;
            }
            index_pre = index_target;

            Move(Vector2.one * -1);

            if (index_target != -1)
            {
                callBack?.Invoke(index_target);
            }

            center_step = 0;
            center_state = index_target != -1;
        }

        private bool Center(Vector2 position)
        {
            bool result = false;

            switch (direction)
            {
                case Direction.Horizontal:
                    result = Math.Abs(position.x - center.x) < cell.x / 2;
                    break;
                case Direction.Vertical:
                    result = Math.Abs(position.y - center.y) < cell.y / 2;
                    break;
            }

            return result;
        }

        private bool Drag { get; set; }

        enum Direction
        {
            None,
            Horizontal,
            Vertical,
        }
    }
}