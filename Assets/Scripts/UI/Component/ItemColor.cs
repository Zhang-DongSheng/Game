using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemColor : ItemBase, IPointerClickHandler
    {
        [SerializeField] private Image imgColor;

        [SerializeField] private Image imgAlpha;

        private Color color = new Color(1, 1, 1, 1);

        private float alpha = 1;

        private void Awake()
        {
            color = imgAlpha.color;

            alpha = imgAlpha.fillAmount;

            color.a = 1;

            RefreshColor(color, alpha);
        }

        public void Refresh(string hexadecimal)
        {
            if (ColorUtility.TryParseHtmlString(hexadecimal, out color))
            {
                alpha = color.a;

                color.a = 1;
            }
            else
            {
                color = Color.white;

                alpha = 1;
            }
            RefreshColor(color, alpha);
        }

        public void Refresh(Color color)
        {
            alpha = color.a;

            color.a = 1;

            RefreshColor(color, alpha);
        }

        private void RefreshColor(Color color, float alpha)
        {
            imgColor.color = color;

            imgAlpha.fillAmount = alpha;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Action<Color> action = Refresh;

            UIQuickEntry.Open(UIPanel.ColorPicker, new UIParameter()
            {
                ["color"] = color,
                ["alpha"] = alpha,
                ["callback"] = action
            });
        }
    }
}