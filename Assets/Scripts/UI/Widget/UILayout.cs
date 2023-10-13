using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UILayout : ItemBase
    {
        private readonly Vector2 TOPLEFT = new Vector2(0, 1);

        [SerializeField] private RectTransform root;

        [SerializeField] private Axis axis;

        [SerializeField] private float interval;

        [SerializeField] private float line;

        private Vector2 space;

        private Vector2 position, temp;

        private readonly List<RectTransform> childs = new List<RectTransform>();

        private void Start()
        {
            Refresh();
        }

        public void Refresh()
        {
            Init();

            switch (axis)
            {
                case Axis.Horizontal:
                    Horizontal();
                    break;
                case Axis.Vertical:
                    Vertical();
                    break;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(root);
        }

        private void Init()
        {
            space = new Vector2(root.rect.width, root.rect.height);

            position = Vector2.zero;

            childs.Clear();

            for (int i = 0; i < root.childCount; i++)
            {
                childs.Add(root.GetChild(i).GetComponent<RectTransform>());
            }
        }

        private void Horizontal()
        {
            int surplus = -1;

            for (int i = 0; i < childs.Count; i++)
            {
                if (position.x + childs[i].rect.width > space.x)
                {
                    position.x = 0;

                    position.y -= line;
                }

                if (position.y < space.y)
                {
                    temp = position;

                    SetPosition(childs[i], position);
                }
                else
                {
                    surplus = i;
                    break;
                }
                position.x += childs[i].rect.width + interval;

                childs[i].SetActive(true);
            }

            if (surplus == -1) return;

            for (int i = surplus; i < childs.Count; i++)
            {
                childs[i].SetActive(false);
            }
        }

        private void Vertical()
        {
            int surplus = -1;

            for (int i = 0; i < childs.Count; i++)
            {
                if (position.y + childs[i].rect.height > space.y)
                {
                    position.x += line;

                    position.y = 0;
                }

                if (position.x < space.x)
                {
                    temp = position;

                    temp.y *= -1f;

                    SetPosition(childs[i], temp);
                }
                else
                {
                    surplus = i;
                    break;
                }
                position.y += childs[i].rect.height + interval;

                childs[i].SetActive(true);
            }

            if (surplus == -1) return;

            for (int i = surplus; i < childs.Count; i++)
            {
                childs[i].SetActive(false);
            }
        }

        private void SetPosition(RectTransform child, Vector2 position)
        {
            child.anchorMin = TOPLEFT;

            child.anchorMax = TOPLEFT;

            child.pivot = TOPLEFT;

            child.anchoredPosition = position;

            child.SetActive(true);
        }
    }
}