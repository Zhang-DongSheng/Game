using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

namespace IronForce2.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class ListLayoutGroup : LayoutGroup
    {
        public Action<Vector2> onDragHandler;

        [SerializeField] protected Corner m_StartCorner = Corner.UpperLeft;
        public Corner startCorner { get { return m_StartCorner; } set { SetProperty(ref m_StartCorner, value); } }

        [SerializeField] protected Vector2 m_CellSize = new Vector2(100, 100);
        public Vector2 cellSize { get { return m_CellSize; } set { SetProperty(ref m_CellSize, value); } }

        [SerializeField] protected Vector2 m_Spacing = Vector2.zero;
        public Vector2 spacing { get { return m_Spacing; } set { SetProperty(ref m_Spacing, value); } }

        [SerializeField] Constraint m_constraint = Constraint.Flexible;
        public Constraint Constraint
        {
            get
            {
                return m_constraint;
            }
            set
            {
                SetProperty(ref m_constraint, value);
            }
        }

        [SerializeField] protected int m_ConstraintCount = 2;
        public int constraintCount { get { return m_ConstraintCount; } set { SetProperty(ref m_ConstraintCount, Mathf.Max(1, value)); } }

        [SerializeField] private GameObject m_template;
        public GameObject template
        {
            get
            {
                return m_template;
            }
            set
            {
                if (m_template != null || value == null)
                    return;
                m_template = value;
                var rect = m_template.GetComponent<RectTransform>();
                rect.anchorMin = Vector2.up;
                rect.anchorMax = Vector2.up;
                rect.sizeDelta = cellSize;
            }
        }

        private ScrollRect m_scrol;

        private Action<int, GameObject, object> m_handler;

        private int displayListCount = 0;

        private bool m_endlessScroll = false;

        private readonly List<int> m_indexes = new List<int>();

        private readonly List<object> m_list = new List<object>();

        public void SetData<P, D>(ICollection<D> data, Action<int, P, D> callback) where P : MonoBehaviour
        {
            if (m_template == null)
            {
                Debug.LogError("Template is null !");
                return;
            }
            if (!CheckScrollRect())
            {
                return;
            }
            SetDataSub(data, callback);
        }

        public void SetData<P, D>(P prefab, ICollection<D> data, Action<int, P, D> callback) where P : MonoBehaviour
        {
            if (!CheckScrollRect())
            {
                return;
            }
            if (prefab != m_template)
            {
                Clear();
            }
            m_template = null;
            template = prefab.gameObject;
            m_handler = null;
            m_scrol.normalizedPosition = Vector2.up;

            SetDataSub(data, callback);
        }

        public void ScrollMoveTo(int index, Action moveDoneCallback = null)
        {
            if (index < 0 || index >= m_list.Count || !gameObject.activeInHierarchy)
                return;
            StartCoroutine(_ScrollMoveTo(index, moveDoneCallback));
        }

        public void ForceUpdateContent()
        {
            m_indexes.Clear();
            OnDrag(Vector2.zero);
        }

        public void Clear()
        {
            if (rectTransform.childCount > 0)
            {
                List<GameObject> goList = new List<GameObject>();
                for (int i = 0; i < rectTransform.childCount; ++i)
                {
                    goList.Add(rectTransform.GetChild(i).gameObject);
                }
                rectTransform.DetachChildren();
                while (goList.Count > 0)
                {
                    var go = goList[goList.Count - 1];
                    GameObject.Destroy(go);
                    goList.RemoveAt(goList.Count - 1);
                }

                goList = null;
            }
            m_list.Clear();
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            int minColumns;

            int preferredColumns;

            if (m_constraint == Constraint.FixedColumnCount)
            {
                minColumns = preferredColumns = m_ConstraintCount;
            }
            else
            {
                minColumns = preferredColumns = GetMaxListCount();
            }
            SetLayoutInputForAxis(
                padding.horizontal + (cellSize.x + spacing.x) * minColumns - spacing.x,
                padding.horizontal + (cellSize.x + spacing.x) * preferredColumns - spacing.x,
                -1, 0);
        }

        public override void CalculateLayoutInputVertical()
        {
            int minRows = 0;
            if (m_constraint == Constraint.FixedColumnCount)
            {
                minRows = GetMaxListCount();
            }
            else
            {
                minRows = m_ConstraintCount;
            }

            float minSpace = padding.vertical + (cellSize.y + spacing.y) * minRows - spacing.y;
            SetLayoutInputForAxis(minSpace, minSpace, -1, 1);
        }

        public override void SetLayoutHorizontal()
        {
            SetCellsAlongAxis(0);
        }

        public override void SetLayoutVertical()
        {
            SetCellsAlongAxis(1);
        }

        protected virtual void OnDrag(Vector2 vec)
        {
            m_endlessScroll = IsEndlessScroll();
            SetLayoutHorizontal();
            SetLayoutVertical();
            if (onDragHandler != null)
                onDragHandler.Invoke(vec);
        }

        private int GetActualIndex(int startIndex, int index)
        {
            var count = rectTransform.childCount;
            return ((startIndex + (startIndex >= 0 ? (count - index - 1) : -index)) / count * count + index);
        }

        private int GetMaxListCount()
        {
            if (m_list == null || m_list.Count == 0)
                return 0;
            return Mathf.CeilToInt((float)m_list.Count / m_ConstraintCount);
        }

        private int GetStartIndex()
        {
            if (m_list == null || m_list.Count == 0)
                return 0;
            Vector2 anchorPosition = rectTransform.anchoredPosition;

            anchorPosition.x *= -1;

            int index = 0;

            anchorPosition.x -= padding.left;
            anchorPosition.y -= padding.top;

            switch (m_scrol.vertical)
            {
                case true:
                    index = (int)(anchorPosition.y / (cellSize.y + spacing.y)) * constraintCount;
                    break;
                case false:
                    index = (int)(anchorPosition.x / (cellSize.x + spacing.x)) * constraintCount;
                    break;
            }

            if (!m_endlessScroll)
            {
                if (index < 0)
                    index = 0;
                if (index >= m_list.Count)
                    index = 0;
            }
            return index;
        }

        private Vector2 GetScrollRectSize()
        {
            return m_scrol.GetComponent<RectTransform>().rect.size;
        }

        private bool CheckScrollRect()
        {
            if (m_scrol != null) return true;

            m_scrol = GetComponentInParent<ScrollRect>();

            if (m_scrol == null)
            {
                Debug.LogError("ScrollRect is null !");
                return false;
            }
            m_scrol.onValueChanged.AddListener(OnDrag);

            return true;
        }

        private bool IsEndlessScroll()
        {
            return m_scrol.movementType == ScrollRect.MovementType.Unrestricted;
        }

        private void SetDataSub<P, D>(ICollection<D> data, Action<int, P, D> callback) where P : MonoBehaviour
        {
            m_list.Clear();
            if (data != null && data.Count > 0)
            {
                var enu = data.GetEnumerator();
                while (enu.MoveNext())
                {
                    m_list.Add(enu.Current);
                }
            }

            if (callback == null)
                m_handler = null;
            else
                m_handler = (index, go, data) =>
                {
                    var item = go.GetComponent<P>();
                    item.name = "item_" + index;
                    callback?.Invoke(index, item, (D)data);
                };

            if (m_scrol.horizontal)
                displayListCount = Mathf.CeilToInt(GetScrollRectSize().x / (cellSize.x + spacing.x)) + 1;
            else
                displayListCount = Mathf.CeilToInt(GetScrollRectSize().y / (cellSize.y + spacing.y)) + 1;

            int count = displayListCount * constraintCount;

            if (data != null && data.Count < count)
            {
                count = data.Count;
            }
            for (int i = rectTransform.childCount; i < count; ++i)
            {
                var obj = AddChild(rectTransform, m_template);
                SetActive(obj.transform, true);
            }
            ForceUpdateContent();

            SetDirty();
        }

        private void SetCellsAlongAxis(int axis)
        {
            // Normally a Layout Controller should only set horizontal values when invoked for the horizontal axis
            // and only vertical values when invoked for the vertical axis.
            // However, in this case we set both the horizontal and vertical position when invoked for the vertical axis.
            // Since we only set the horizontal position and not the size, it shouldn't affect children's layout,
            // and thus shouldn't break the rule that all horizontal layout must be calculated before all vertical layout.
            if (axis == 0)
            {
                // Only set the sizes when invoked for horizontal axis, not the positions.
                for (int i = 0; i < rectTransform.childCount; i++)
                {
                    RectTransform rect = (RectTransform)rectTransform.GetChild(i);

                    m_Tracker.Add(this, rect,
                        DrivenTransformProperties.Anchors |
                        DrivenTransformProperties.AnchoredPosition |
                        DrivenTransformProperties.SizeDelta);

                    rect.anchorMin = Vector2.up;
                    rect.anchorMax = Vector2.up;
                    rect.sizeDelta = cellSize;
                    rect.localScale = Vector3.one;
                }
                return;
            }

            float width = rectTransform.rect.size.x;
            float height = rectTransform.rect.size.y;

            int cellCountX = 1;
            int cellCountY = 1;
            if (m_constraint == Constraint.FixedColumnCount)
            {
                cellCountX = m_ConstraintCount;
                cellCountY = Mathf.CeilToInt(rectTransform.childCount / (float)cellCountX - 0.001f);
            }
            else if (m_constraint == Constraint.FixedRowCount)
            {
                cellCountY = m_ConstraintCount;
                cellCountX = Mathf.CeilToInt(rectTransform.childCount / (float)cellCountY - 0.001f);
            }
            else
            {
                if (cellSize.x + spacing.x <= 0)
                    cellCountX = int.MaxValue;
                else
                    cellCountX = Mathf.Max(1, Mathf.FloorToInt((width - padding.horizontal + spacing.x + 0.001f) / (cellSize.x + spacing.x)));

                if (cellSize.y + spacing.y <= 0)
                    cellCountY = int.MaxValue;
                else
                    cellCountY = Mathf.Max(1, Mathf.FloorToInt((height - padding.vertical + spacing.y + 0.001f) / (cellSize.y + spacing.y)));
            }

            int cornerX = (int)startCorner % 2;
            int cornerY = (int)startCorner / 2;

            int cellsPerMainAxis, actualCellCountX, actualCellCountY;

            if (m_constraint == Constraint.FixedColumnCount)
            {
                cellsPerMainAxis = cellCountX;
                actualCellCountX = Mathf.Clamp(cellCountX, 1, rectTransform.childCount);
                actualCellCountY = Mathf.Clamp(cellCountY, 1, Mathf.CeilToInt((float)rectTransform.childCount / cellsPerMainAxis));
            }
            else if (m_constraint == Constraint.FixedRowCount)
            {
                cellsPerMainAxis = cellCountY;
                actualCellCountY = Mathf.Clamp(cellCountY, 1, rectTransform.childCount);
                actualCellCountX = Mathf.Clamp(cellCountX, 1, Mathf.CeilToInt((float)rectTransform.childCount / cellsPerMainAxis));
            }
            else
            {
                cellsPerMainAxis = cellCountY;
                actualCellCountY = Mathf.Clamp(cellCountY, 1, rectTransform.childCount);
                actualCellCountX = Mathf.Clamp(cellCountX, 1, Mathf.CeilToInt((float)rectTransform.childCount / cellsPerMainAxis));
            }

            Vector2 requiredSpace = new Vector2(
                                         actualCellCountX * cellSize.x + (actualCellCountX - 1) * spacing.x,
                                         actualCellCountY * cellSize.y + (actualCellCountY - 1) * spacing.y
                                     );
            Vector2 startOffset = new Vector2(
                                       GetStartOffset(0, requiredSpace.x),
                                       GetStartOffset(1, requiredSpace.y)
                                   );

            int startIndex = GetStartIndex();
            bool changed = false;
            for (int i = 0; i < rectTransform.childCount; i++)
            {
                int positionX;
                int positionY;
                int actualIndex = GetActualIndex(startIndex, i - constraintCount);
                int recycleIndex = m_endlessScroll ? actualIndex % m_list.Count : actualIndex;
                if (actualIndex < 0)
                    actualIndex -= constraintCount - 1;
                if (i >= m_indexes.Count)
                {
                    m_indexes.Add(actualIndex);
                    changed = true;
                }
                else
                    changed = (m_indexes[i] != actualIndex);
                m_indexes[i] = actualIndex;
                if (!changed)
                    continue;
                while (recycleIndex < 0)
                    recycleIndex += m_list.Count;
                var child = (RectTransform)(rectTransform.GetChild(i));
                if (recycleIndex >= m_list.Count)
                {
                    SetActive(child, false);
                    continue;
                }
                SetActive(child, true);
                if (m_constraint == Constraint.FixedColumnCount)
                {
                    positionX = Mathf.Abs(actualIndex) % cellsPerMainAxis;
                    positionY = actualIndex / cellsPerMainAxis;
                }
                else
                {
                    positionX = actualIndex / cellsPerMainAxis;
                    positionY = Mathf.Abs(actualIndex) % cellsPerMainAxis;
                }

                if (cornerX == 1)
                    positionX = actualCellCountX - 1 - positionX;
                if (cornerY == 1)
                    positionY = actualCellCountY - 1 - positionY;
                if (m_handler != null && changed)
                {
                    m_handler.Invoke(recycleIndex, child.gameObject, m_list[recycleIndex]);
                }
                SetChildAlongAxis(child, 0, startOffset.x + (cellSize[0] + spacing[0]) * positionX, cellSize[0]);
                SetChildAlongAxis(child, 1, startOffset.y + (cellSize[1] + spacing[1]) * positionY, cellSize[1]);
            }
        }

        private GameObject AddChild(Transform parent, GameObject prefab)
        {
            var clone = GameObject.Instantiate(prefab, parent);

            clone.transform.localPosition = Vector3.zero;

            clone.transform.localRotation = Quaternion.identity;

            clone.transform.localScale = Vector3.one;

            return clone;
        }

        private IEnumerator _ScrollMoveTo(int index, Action callback = null)
        {
            yield return new WaitForEndOfFrame();

            Vector3 itemPos = GetItemPosLeftUp(index);

            float vContent = 0, vViewport = 0, delta = 0, threshold = 0, targetPos;
            if (m_scrol.horizontal)
            {
                //水平方向
                vContent = rectTransform.rect.width;
                vViewport = m_scrol.viewport != null ? m_scrol.viewport.rect.width : m_scrol.GetComponent<RectTransform>().rect.width;
                delta = itemPos.x - rectTransform.anchoredPosition.x;
                targetPos = itemPos.x;
            }
            else
            {
                //非水平方向 当做竖直方向处理
                vContent = rectTransform.rect.height;
                vViewport = m_scrol.viewport != null ? m_scrol.viewport.rect.height : m_scrol.GetComponent<RectTransform>().rect.height;
                delta = rectTransform.anchoredPosition.y - itemPos.y;
                targetPos = itemPos.y;
            }
            //计算出滑动的阈值（滑动到多少的距离，就不能滑动了）
            threshold = vContent - vViewport;
            float normalizedPos = 0;
            if (delta >= threshold)
            {
                //已经滑到底了
                normalizedPos = 0;
            }
            else
            {
                normalizedPos = 1 / threshold * targetPos;
            }
            normalizedPos = Mathf.Clamp01(normalizedPos);
            if (m_scrol.horizontal)
                m_scrol.horizontalNormalizedPosition = normalizedPos;
            else
                m_scrol.verticalNormalizedPosition = 1 - normalizedPos;
            if (callback != null)
            {
                callback();
            }
        }

        private Vector3 GetItemPosLeftUp(int dataIndex)
        {
            int line = dataIndex / constraintCount;
            Rect contentRect = rectTransform.rect;
            //起始点就是content的左上角的点
            Vector2 startPos = new Vector2(contentRect.xMin, contentRect.yMax);
            //取得item左上角的位置
            if (m_scrol.horizontal)
            {
                startPos.x += padding.left;
                startPos.x += line * (cellSize.x + spacing.x);
            }
            else
            {
                //非水平方向按照竖直方向处理
                startPos.y += padding.top;
                startPos.y += line * (cellSize.y + spacing.y);
            }
            return startPos;
        }

        private void SetActive(Component component, bool active)
        {
            if (component == null) return;

            var go = component.gameObject;

            if (go != null && go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }

        protected override void OnDestroy()
        {
            Clear();
            m_list.Clear();
            m_indexes.Clear();
        }
    }
}