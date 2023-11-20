using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IronForce2.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class ListLayoutGroup : LayoutGroup
    {
        public enum Corner
        {
            UpperLeft = 0,
            UpperRight = 1,
            LowerLeft = 2,
            LowerRight = 3

        }

        public enum Axis
        {
            Horizontal = 0,
            Vertical = 1

        }

        public enum Constraint
        {
            FixedColumnCount = 0,
            FixedRowCount = 1

        }

        [SerializeField] protected Corner m_StartCorner = Corner.UpperLeft;

        public Corner startCorner { get { return m_StartCorner; } set { SetProperty(ref m_StartCorner, value); } }


        [SerializeField] protected Vector2 m_CellScale = new Vector2(1, 1);

        public Vector2 cellScale { get { return m_CellScale; } set { SetProperty(ref m_CellScale, value); } }


        [SerializeField] protected Vector2 m_CellSize = new Vector2(100, 100);

        public Vector2 cellSize { get { return m_CellSize; } set { SetProperty(ref m_CellSize, value); } }

        [SerializeField] protected Vector2 m_Spacing = Vector2.zero;

        public Vector2 spacing { get { return m_Spacing; } set { SetProperty(ref m_Spacing, value); } }


        [SerializeField] protected int m_ConstraintCount = 2;


        public int constraintCount { get { return m_ConstraintCount; } set { SetProperty(ref m_ConstraintCount, Mathf.Max(1, value)); } }

        public Axis m_scrollDirection = Axis.Horizontal;

        protected Constraint m_Constraint = Constraint.FixedColumnCount;
        [SerializeField] protected bool m_autoSetConstraint = true;

        [SerializeField] Constraint m_dataConstraint = Constraint.FixedColumnCount;
        public Constraint DataConstraint
        {
            get
            {
                return m_dataConstraint;
            }
            set
            {
                SetProperty(ref m_dataConstraint, value);
            }
        }
        public ScrollRect m_scrollRect;

        [HideInInspector]
        public List<object> m_dataList = new List<object>();

        [HideInInspector]
        public List<object> m_cacheList = new List<object>();

        private int displayListCount = 0;

        private System.Action<int, GameObject, object> m_setContentHandler = null;

        [SerializeField]
        private MonoBehaviour m_objectTemplate;

        private List<int> m_indexList = new List<int>();

        //private Vector2 m_dragDirection = Vector2.zero;

        private bool m_endlessScroll = false;

        public LayoutGroup layoutGroup;

        public Action<Vector2> onDragHandler;
        public MonoBehaviour objectTemplate
        {
            set
            {
                if (m_objectTemplate != null || value == null)
                    return;
                m_objectTemplate = value;
                var rect = m_objectTemplate.GetComponent<RectTransform>();
                rect.anchorMin = Vector2.up;
                rect.anchorMax = Vector2.up;
                rect.sizeDelta = cellSize;
                rect.localScale = cellScale;
            }
            get
            {
                return m_objectTemplate;
            }
        }

        public void OnDestroy()
        {
            clearNodeList();


            m_dataList.Clear();
            m_cacheList.Clear();
            m_indexList.Clear();

            m_setContentHandler = null;
            m_objectTemplate = null;
            layoutGroup = null;
            onDragHandler = null;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            constraintCount = constraintCount;
        }
#endif

        private Vector2 GetScrollRectSize()
        {
            return m_scrollRect.GetComponent<RectTransform>().rect.size;
        }

        protected override void Awake()
        {
            base.Awake();
            setScrollRect();
        }

        public void setScrollRect()
        {
            m_scrollRect = GetComponentInParent<ScrollRect>();
            if (m_scrollRect == null)
            {
                m_scrollRect = GetComponentInParent<ScrollRect>();
            }

            if (m_scrollRect != null)
            {
                m_scrollDirection = m_scrollRect.vertical ? Axis.Vertical : Axis.Horizontal;
                m_scrollRect.onValueChanged.AddListener(OnDrag);
            }
        }

        private bool IsEndlessScroll()
        {
            return m_scrollRect.movementType == ScrollRect.MovementType.Unrestricted;
        }

        public void clearNodeList()
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
                    GameObject.Destroy( go);
                    goList.RemoveAt(goList.Count - 1);
                }

                goList = null;
            }

            m_dataList.Clear();
            m_cacheList.Clear();
            objectTemplate = null;
        }

        public void SetData<P, D>(P itemPrefab, ICollection<D> dataList, System.Action<int, P, D> setContentHandler) where P : MonoBehaviour
        {
            if (itemPrefab != m_objectTemplate)
            {
                clearNodeList();
            }
            m_objectTemplate = null;
            objectTemplate = itemPrefab;
            m_setContentHandler = null;

            if (m_scrollRect == null)
            {
                setScrollRect();
            }


            if (m_scrollRect != null)
            {
                m_scrollRect.normalizedPosition = Vector2.up;
                SetDataSub<P, D>(dataList, setContentHandler);
            }
            else
            {
                Debug.LogError("tran=" + transform + "的ScrollRect为空");
            }
        }

        public void ILSetData(MonoBehaviour itemPrefab, int count, System.Action<int, MonoBehaviour> setContentHandler)
        {
            if (itemPrefab != m_objectTemplate)
            {
                clearNodeList();
            }
            m_objectTemplate = null;
            objectTemplate = itemPrefab;
            m_setContentHandler = null;

            if (m_scrollRect == null)
            {
                setScrollRect();
            }


            if (m_scrollRect != null)
            {
                m_scrollRect.normalizedPosition = Vector2.up;
                ILSetDataSub(count, setContentHandler);
            }
            else
            {
                Debug.LogError("tran=" + transform + "的ScrollRect为空");
            }
        }

        public void SetData<P, D>(ICollection<D> dataList, Action<int, P, D> setContentHandler) where P : MonoBehaviour
        {
            if (m_objectTemplate == null)
            {
                Debug.LogError("template is null !");
                return;
            }
            SetDataSub<P, D>(dataList, setContentHandler);
        }

        public void SetDataSub<P, D>(ICollection<D> dataList, System.Action<int, P, D> setContentHandler)where P : MonoBehaviour
        {
            m_dataList.Clear();
            if (dataList != null && dataList.Count > 0)
            {
                var enu = dataList.GetEnumerator();
                while (enu.MoveNext())
                {
                    m_dataList.Add(enu.Current);
                }
            }

            if (setContentHandler == null)
                m_setContentHandler = null;
            else
                m_setContentHandler = (index, go, data) =>
                {

                    var dt = (D)data;
                    var item = go.GetComponent<P>();
                    item.name = "item_" + index;

                    if (setContentHandler != null)
                    {
                        setContentHandler.Invoke(index, item, dt);
                    }
                };
            m_scrollRect.vertical = (m_scrollDirection == Axis.Vertical);
            m_scrollRect.horizontal = (m_scrollDirection == Axis.Horizontal);
            m_Constraint = m_scrollRect.vertical ? Constraint.FixedColumnCount : Constraint.FixedRowCount;
            if (!m_autoSetConstraint && m_Constraint != m_dataConstraint)
            {
                m_cacheList.Clear();
                m_cacheList.AddRange(m_dataList);
                int lineCount = m_cacheList.Count / constraintCount;
                for (int i = 0; i < m_cacheList.Count; ++i)
                {
                    int x = i / constraintCount;
                    int y = i % constraintCount;
                    m_dataList[i] = m_cacheList[y * lineCount + x];
                }
            }
            if (m_scrollRect.horizontal)
                displayListCount = Mathf.CeilToInt(GetScrollRectSize().x / (cellSize.x + spacing.x)) + 1;
            else
                displayListCount = Mathf.CeilToInt(GetScrollRectSize().y / (cellSize.y + spacing.y)) + 1;

            int count = displayListCount * constraintCount;
            if (dataList != null && dataList.Count < count)
            {
                count = dataList.Count;
            }

            for (int i = rectTransform.childCount; i < count; ++i)
            {
                var obj = AddChild(rectTransform, m_objectTemplate);
                SetActive(obj, true);
            }

            ForceUpdateContent();
            SetDirty();
        }

        public void ILSetDataSub(int _count, System.Action<int, MonoBehaviour> setContentHandler)
        {
            m_dataList.Clear();

            if (_count > 0)
            {
                for(int i=0; i<_count; i++)
                {
                    m_dataList.Add(i);
                }
            }

            if (setContentHandler == null)
                m_setContentHandler = null;
            else
                m_setContentHandler = (index, go, data) =>
                {
                    var item = go.GetComponent<MonoBehaviour>();
                    item.name = "item_" + index;

                    if (setContentHandler != null)
                    {
                        setContentHandler.Invoke(index, item);
                    }
                };
            m_scrollRect.vertical = (m_scrollDirection == Axis.Vertical);
            m_scrollRect.horizontal = (m_scrollDirection == Axis.Horizontal);
            m_Constraint = m_scrollRect.vertical ? Constraint.FixedColumnCount : Constraint.FixedRowCount;
            if (!m_autoSetConstraint && m_Constraint != m_dataConstraint)
            {
                m_cacheList.Clear();
                m_cacheList.AddRange(m_dataList);
                int lineCount = m_cacheList.Count / constraintCount;
                for (int i = 0; i < m_cacheList.Count; ++i)
                {
                    int x = i / constraintCount;
                    int y = i % constraintCount;
                    m_dataList[i] = m_cacheList[y * lineCount + x];
                }
            }
            if (m_scrollRect.horizontal)
                displayListCount = Mathf.CeilToInt(GetScrollRectSize().x / (cellSize.x + spacing.x)) + 1;
            else
                displayListCount = Mathf.CeilToInt(GetScrollRectSize().y / (cellSize.y + spacing.y)) + 1;

            int count = displayListCount * constraintCount;
            if (_count < count)
            {
                count = _count;
            }

            for (int i = rectTransform.childCount; i < count; ++i)
            {
                var obj = AddChild(rectTransform, m_objectTemplate);
                SetActive(obj, true);
            }

            ForceUpdateContent();
            SetDirty();
        }


        public void ForceUpdateContent()
        {
            m_indexList.Clear();
            OnDrag(Vector2.zero);
        }

        protected virtual void OnDrag(Vector2 vec)
        {
            //m_dragDirection = vec;
            m_endlessScroll = IsEndlessScroll();
            SetLayoutHorizontal();
            SetLayoutVertical();
            if (onDragHandler != null)
                onDragHandler.Invoke(vec);
        }

        public int GetMaxListCount()
        {
            if (m_dataList == null || m_dataList.Count == 0)
                return 0;
            return Mathf.CeilToInt(m_dataList.Count / (float)m_ConstraintCount - 0.001f);
        }
        public int GetStartIndex()
        {
            if (m_dataList == null || m_dataList.Count == 0)
                return 0;
            Vector2 anchorPosition = rectTransform.anchoredPosition;

            if (layoutGroup != null)
            {
                anchorPosition += layoutGroup.GetComponent<RectTransform>().anchoredPosition;
            }

            anchorPosition.x *= -1;
            //anchorPosition.x -= (m_dragDirection.x > 0 ? 1 : 2) * (cellSize.x + spacing.x) / 2;
            //anchorPosition.y -= (m_dragDirection.y > 0 ? 1 : 2) * (cellSize.y + spacing.y) / 2;
            int index = 0;

            anchorPosition.x -= padding.left;
            anchorPosition.y -= padding.top;

            switch (m_scrollRect.vertical)
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
                if (index >= m_dataList.Count)
                    index = 0;
            }
            return index;
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            int minColumns = 0;
            int preferredColumns = 0;
            if (m_Constraint == Constraint.FixedColumnCount)
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
            if (m_Constraint == Constraint.FixedColumnCount)
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

/***
        internal void SetData1<T1, T2>(T1 backItem, List<SubTankObj> rightList, Action<int, T1, T2> p)
        {
            throw new NotImplementedException();
        }
**/
        public override void SetLayoutHorizontal()
        {
            SetCellsAlongAxis(0);
        }

        public override void SetLayoutVertical()
        {
            SetCellsAlongAxis(1);
        }

        //return true if index of current item changed
        private int GetActualIndex(int startIndex, int index)
        {
            var count = rectTransform.childCount;
            return ((startIndex + (startIndex >= 0 ? (count - index - 1) : -index)) / count * count + index);
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
                    rect.localScale = cellScale;
                }
                return;
            }

            float width = rectTransform.rect.size.x;
            float height = rectTransform.rect.size.y;

            int cellCountX = 1;
            int cellCountY = 1;
            if (m_Constraint == Constraint.FixedColumnCount)
            {
                cellCountX = m_ConstraintCount;
                cellCountY = Mathf.CeilToInt(rectTransform.childCount / (float)cellCountX - 0.001f);
            }
            else if (m_Constraint == Constraint.FixedRowCount)
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
            if (m_scrollDirection == Axis.Vertical)
            {
                cellsPerMainAxis = cellCountX;
                actualCellCountX = Mathf.Clamp(cellCountX, 1, rectTransform.childCount);
                actualCellCountY = Mathf.Clamp(cellCountY, 1, Mathf.CeilToInt(rectTransform.childCount / (float)cellsPerMainAxis));
            }
            else
            {
                cellsPerMainAxis = cellCountY;
                actualCellCountY = Mathf.Clamp(cellCountY, 1, rectTransform.childCount);
                actualCellCountX = Mathf.Clamp(cellCountX, 1, Mathf.CeilToInt(rectTransform.childCount / (float)cellsPerMainAxis));
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
                int recycleIndex = m_endlessScroll ? actualIndex % m_dataList.Count : actualIndex;
                if (actualIndex < 0)
                    actualIndex -= constraintCount - 1;
                if (i >= m_indexList.Count)
                {
                    m_indexList.Add(actualIndex);
                    changed = true;
                }
                else
                    changed = (m_indexList[i] != actualIndex);
                m_indexList[i] = actualIndex;
                if (!changed)
                    continue;
                while (recycleIndex < 0)
                    recycleIndex += m_dataList.Count;
                var child = (RectTransform)(rectTransform.GetChild(i));
                if (recycleIndex >= m_dataList.Count)
                {
                    SetActive(child, false);
                    continue;
                }
                SetActive(child, true);
                if (m_scrollDirection == Axis.Vertical)
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
                if (m_setContentHandler != null && changed)
                {
                    m_setContentHandler.Invoke(recycleIndex, child.gameObject, m_dataList[recycleIndex]);
                }
                SetChildAlongAxis(child, 0, startOffset.x + (cellSize[0] + spacing[0]) * positionX, cellSize[0]);
                SetChildAlongAxis(child, 1, startOffset.y + (cellSize[1] + spacing[1]) * positionY, cellSize[1]);
            }
        }

        public int GetIndexDistance(int index, int actualIndex)
        {
            while (actualIndex < 0)
                actualIndex += m_indexList.Count;
            return index - actualIndex;
        }

        public void GetXYIndex(int dataIndex, int firstIndex, out int xIndex, out int yIndex)
        {
            xIndex = dataIndex % constraintCount - firstIndex % constraintCount;
            yIndex = dataIndex / constraintCount - firstIndex / constraintCount;
        }

        public Vector3 GetItemPosition(int dataIndex)
        {
            if (m_indexList.Count == 0)
                return Vector3.zero;
            int firstIndex = -1;
            for (int i = 0; i < rectTransform.childCount; ++i)
            {
                if (rectTransform.GetChild(i).gameObject.activeSelf)
                {
                    firstIndex = i;
                    break;
                }
            }
            if (firstIndex < 0)
                return Vector3.zero;
            //int indexDistance = GetIndexDistance (dataIndex, m_indexList [firstIndex]);
            int xIndexDistance;
            int yIndexDistance;
            GetXYIndex(dataIndex, m_indexList[firstIndex], out xIndexDistance, out yIndexDistance);

            var pos = rectTransform.GetChild(firstIndex).GetComponent<RectTransform>().anchoredPosition3D;

            pos.x += xIndexDistance * (cellSize.x + spacing.x);
            pos.y -= yIndexDistance * (cellSize.y + spacing.y);

            return pos;

        }

        public Vector3 GetItemPositionInScroll(int dataIndex)
        {
            return GetItemPosition(dataIndex) + rectTransform.anchoredPosition3D;
        }

        /// <summary>
        /// 跳转至数据下标处 待测试
        /// </summary>
        /// <param name="dataIndex"></param>
        public void TurnToIndex(int dataIndex)
        {
            if (m_dataList == null || m_dataList.Count - 1 < dataIndex || layoutGroup == null)
                return;

            rectTransform.anchoredPosition = GetItemPositionInScroll(dataIndex);
        }

        private T AddChild<T>(Transform parent, T prefab) where T : Component
        {
            var go = GameObject.Instantiate(prefab, parent);

            go.transform.localPosition = Vector3.zero;

            go.transform.localRotation = Quaternion.identity;

            go.transform.localScale = Vector3.one;

            return go.GetComponent<T>();
        }

        public void ScrollMoveTo(int index, System.Action moveDoneCallback = null)
        {
            if (index < 0 || index >= m_dataList.Count || !gameObject.activeInHierarchy)
                return;
            StartCoroutine(_ScrollMoveTo(index, moveDoneCallback));
        }
        public void ScrollMoveTo(object data, System.Action moveDoneCallback = null)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            ScrollMoveTo(m_dataList.FindIndex((d) => { return data.Equals(d); }), moveDoneCallback);
        }
        private Vector3 GetItemPosLeftUp(int dataIndex)
        {
            int line = dataIndex / constraintCount;
            Rect contentRect = rectTransform.rect;
            //起始点就是content的左上角的点
            Vector2 startPos = new Vector2(contentRect.xMin, contentRect.yMax);
            //取得item左上角的位置
            if (m_scrollRect.horizontal)
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

        public void SetActive(Component component, bool active)
        {
            if (component == null) return;

            var go = component.gameObject;

            if (go != null && go.activeSelf != active)
            { 
                go.SetActive(active);
            }
        }

        /// <summary>
        /// UI的更新是在LateUpdate中执行，所以必须等待到这一帧结束以后，才会更改scrollRect的content的size
        /// </summary>
        IEnumerator _ScrollMoveTo(int index, System.Action moveDoneCallback = null)
        {
            yield return new WaitForEndOfFrame();

            Vector3 itemPos = GetItemPosLeftUp(index);

            float vContent = 0, vViewport = 0, delta = 0, threshold = 0, targetPos;
            if (m_scrollRect.horizontal)
            {
                //水平方向
                vContent = rectTransform.rect.width;
                vViewport = m_scrollRect.viewport != null ? m_scrollRect.viewport.rect.width : m_scrollRect.GetComponent<RectTransform>().rect.width;
                delta = itemPos.x - rectTransform.anchoredPosition.x;
                targetPos = itemPos.x;
            }
            else
            {
                //非水平方向 当做竖直方向处理
                vContent = rectTransform.rect.height;
                vViewport = m_scrollRect.viewport != null ? m_scrollRect.viewport.rect.height : m_scrollRect.GetComponent<RectTransform>().rect.height;
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
            if (m_scrollRect.horizontal)
                m_scrollRect.horizontalNormalizedPosition = normalizedPos;
            else
                m_scrollRect.verticalNormalizedPosition = 1 - normalizedPos;
            if (moveDoneCallback != null)
            {
                moveDoneCallback();
            }
        }
    }

    public interface IListItem<D>
    {
        void SetData(D data);
    }
}