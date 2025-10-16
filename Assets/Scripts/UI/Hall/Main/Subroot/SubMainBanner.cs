using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SubMainBanner : ItemBase
    {
        [SerializeField] private ScrollRect scroll;

        [SerializeField] private ScrollPage page;

        [SerializeField] private Button next;

        [SerializeField] private Button prev;

        [SerializeField] private RectTransform content;

        private int index, count;

        private readonly List<RectTransform> tabs = new List<RectTransform>();

        private void Awake()
        {
            page.onValueChanged.AddListener(OnValueChanged);

            prev.onClick.AddListener(OnClickPrev);

            next.onClick.AddListener(OnClickNext);

            for (int i = 0; i < content.childCount; i++)
            {
                tabs.Add(content.GetChild(i).GetComponent<RectTransform>());
            }
        }

        private void Start()
        {
            count = scroll.content.childCount;

            index = 0;

            page.DirectionImmediately(index);
        }

        private void RefreshArrow()
        {
            prev.gameObject.SetActive(index > 0);

            next.gameObject.SetActive(index < count - 1);
        }

        private void OnClickPrev()
        {
            if (index > 0)
            {
                index--;
            }
            OnClickTab(index);
        }

        private void OnClickNext()
        {
            if (index < count - 1)
            {
                index++;
            }
            OnClickTab(index);
        }

        private void OnClickTab(int index)
        {
            page.Direction(index);
        }

        private void OnValueChanged(int index)
        {
            this.index = index;

            int count = tabs.Count;

            for (int i = 0; i < count; i++)
            {
                tabs[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, index == i ? 100 : 50);
            }
            LayoutRebuilder.MarkLayoutForRebuild(content);

            RefreshArrow();
        }
    }
}