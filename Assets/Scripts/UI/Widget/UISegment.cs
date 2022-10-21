using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public class UISegment : MonoBehaviour
    {
        public Action<GameObject, object> callback;

        [SerializeField] private PrefabAndParent prefab;

        [SerializeField] private ScrollRect scroll;

        [SerializeField] private RectTransform content;

        [SerializeField] private int number;

        protected IList list;

        protected int step;

        protected SegmentStatus status;

        private readonly List<GameObject> items = new List<GameObject>();

        private void Awake()
        {
            scroll.onValueChanged.AddListener((value) =>
            {
                if (value.y <= 0)
                {
                    OnScrollEnd();
                }
            });
        }

        private void OnScrollEnd()
        {
            if (list == null) return;

            if (status == SegmentStatus.Update ||
                status == SegmentStatus.Complete) return;

            step++;

            Refresh();
        }

        public void Refresh(IList list)
        {
            this.list = list;

            step = 1;

            Refresh();
        }

        protected void Refresh()
        {
            int count = step * number;

            StartCoroutine(Refresh(count));
        }

        protected IEnumerator Refresh(int count)
        {
            status = SegmentStatus.Update;

            for (int i = 0; i < count; i++)
            {
                if (i >= list.Count) break;

                if (i >= items.Count)
                {
                    items.Add(prefab.Create());

                    yield return new WaitForEndOfFrame();
                }
                if (items[i] != null)
                {
                    callback?.Invoke(items[i], list[i]);
                }
                SetActive(items[i], true);
            }
            for (int i = count; i < items.Count; i++)
            {
                SetActive(items[i], false);
            }

            status = list.Count > count ? SegmentStatus.None : SegmentStatus.Complete;

            yield return null;
        }

        protected void SetActive(GameObject item, bool active)
        {
            if (item != null && item.activeSelf != active)
            {
                item.SetActive(active);
            }
        }

        public enum SegmentStatus
        {
            None,
            Update,
            Complete,
        }
    }
}