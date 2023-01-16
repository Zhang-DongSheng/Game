using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public static partial class Extension
    {
        /// <summary>
        /// 设置带省略号文本...
        /// </summary>
        public static void SetTextWithEllipsis(this Text component, string content)
        {
            TextGenerator generator = new TextGenerator();

            RectTransform rectTransform = component.GetComponent<RectTransform>();

            TextGenerationSettings settings = component.GetGenerationSettings(rectTransform.rect.size);

            generator.Populate(content, settings);

            int count = generator.characterCountVisible;

            string value = content;

            if (content.Length > count)
            {
                value = content.Substring(0, count - 3) + "...";
            }

            component.text = value;
        }
        /// <summary>
        /// 设置带省略号文本...
        /// </summary>
        public static void SetTextWithEllipsis(this TextMeshProUGUI component, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                component.text = string.Empty;
            }
            else
            {
                component.text = content;

                component.ForceMeshUpdate();

                string value = component.GetParsedText();

                if (string.IsNullOrEmpty(value)) return;

                if (value.Length < 3) return;

                if (value.Length < content.Length)
                {
                    value = content.Substring(0, value.Length - 3) + "...";
                }

                component.text = value;
            }
        }
        /// <summary>
        /// 设置文本横向显示区域
        /// </summary>
        public static float SetTextWithWidth(this Text component, string content, float space = 0, float min = -1, float max = -1)
        {
            component.text = content;

            float width = component.preferredWidth;

            width += space;

            if (min != -1) width = Math.Max(width, min);

            if (max != -1) width = Math.Min(width, max);

            return width;
        }
        /// <summary>
        /// 设置文本纵向显示区域
        /// </summary>
        public static float SetTextWithHeight(this Text component, string content, float space = 0, float min = -1, float max = -1)
        {
            component.text = content;

            float height = component.preferredHeight;

            height += space;

            if (min != -1) height = Math.Max(height, min);

            if (max != -1) height = Math.Min(height, max);

            return height;
        }
        /// <summary>
        /// 换行
        /// </summary>
        public static void LineBreak(this Text component)
        {
            if (component.supportRichText == false) return;

            if (string.IsNullOrEmpty(component.text)) return;

            string content = component.text;

            content = content.Replace("\\n", "\n");

            component.text = content;
        }

        public static void SetText(this Text component, int value)
        {
            SetText(component, string.Format("{0}", value));
        }

        public static void SetText(this Text component, float value, int digits = -1)
        {
            if (digits > -1)
            {
                SetText(component, string.Format("{0}", Math.Round(value, digits)));
            }
            else
            {
                SetText(component, string.Format("{0}", value));
            }
        }

        public static void SetText(this Text component, long value)
        {
            SetText(component, string.Format("{0}", value));
        }

        public static void SetText(this Text component, UnityEngine.Object value)
        {
            SetText(component, string.Format("{0}", value));
        }

        public static void SetText(this Text component, string value)
        {
            if (component.text != value)
            {
                component.text = value;
            }
        }
    }
}