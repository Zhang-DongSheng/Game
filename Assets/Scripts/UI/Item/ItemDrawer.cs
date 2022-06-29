using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemDrawer : ItemBase
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private float height;

        [SerializeField, Range(0, 3)] private int row;

        [SerializeField] private GridLayoutGroup layout;

        [SerializeField] private Button button;

        [SerializeField] private List<GameObject> status;

        public Action callback;

        private int _min, _max;

        private float min, max;

        private int show, hide;

        private bool active;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Refresh(bool active)
        {
            int count = layout.transform.childCount;

            int row = Mathf.CeilToInt(count / (float)layout.constraintCount);

            _min = Mathf.Min(this.row, row);

            _max = row;

            show = count;

            hide = _min * layout.constraintCount;

            bool overflow = _max > _min;

            min = height;

            min += _min * layout.cellSize.y;

            while (_min > 1)
            {
                _min--;

                min += layout.spacing.y;
            }

            max = height;

            max += _max * layout.cellSize.y;

            while (_max > 1)
            {
                _max--;

                max += layout.spacing.y;
            }

            if (overflow)
            {
                Turn(active);
            }
            button.gameObject.SetActive(overflow);
        }

        private void OnClick()
        {
            Turn(!active);
        }

        private void Turn(bool active)
        {
            this.active = active;

            for (int i = 0; i < status.Count; i++)
            {
                if (status[i] != null)
                {
                    status[i].SetActive(active ? i == 1 : i == 0);
                }
            }

            int count = active ? show : hide;

            GameObject child;

            for (int i = 0; i < layout.transform.childCount; i++)
            {
                child = layout.transform.GetChild(i).gameObject;

                if (i < count)
                {
                    if (child.activeSelf != true)
                    {
                        child.SetActive(true);
                    }
                }
                else
                {
                    if (child.activeSelf == true)
                    {
                        child.SetActive(false);
                    }
                }
            }

            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, active ? max : min);

            callback?.Invoke();
        }
    }
}