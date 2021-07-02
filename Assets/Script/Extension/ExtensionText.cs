using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Game
{
    public static partial class Extension
    {
        private const char special = '#';

        private static readonly Regex emojiRegex = new Regex(@"<emoji=(.+?)/>", RegexOptions.Singleline);

        private static readonly Regex specialRegex = new Regex(string.Format(@"X{0}", special), RegexOptions.Singleline);
        /// <summary>
        /// 设置带图片文本
        /// </summary>
        public static void SetTextWithEmoji(this Text compontent, string content)
        {
            compontent.transform.Clear();

            if (emojiRegex.IsMatch(content))
            {
                List<EmojiInformation> emojis = new List<EmojiInformation>();

                string value = emojiRegex.Replace(content, string.Format("X{0}", special));

                int index = 0;

                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] == special)
                    {
                        emojis.Add(new EmojiInformation()
                        {
                            index = i,
                            size = Vector2.one * compontent.fontSize,
                        });
                    }
                }

                foreach (Match match in emojiRegex.Matches(content))
                {
                    emojis[index++].name = match.Groups[1].Value;
                }

                TextGenerator generator = new TextGenerator();

                RectTransform rectTransform = compontent.GetComponent<RectTransform>();

                TextGenerationSettings settings = compontent.GetGenerationSettings(rectTransform.rect.size);

                generator.Populate(value, settings);

                for (int i = 0; i < emojis.Count; i++)
                {
                    if (generator.characters.Count > emojis[i].index)
                    {
                        emojis[i].position = generator.characters[emojis[i].index].cursorPos;

                        CreateImageEmoji(emojis[i], compontent.transform);
                    }
                }

                compontent.text = specialRegex.Replace(value, string.Format("<color=#FFFFFF00>X{0}</color>", special));
            }
            else
            {
                compontent.text = content;
            }
        }
        /// <summary>
        /// 设置带省略号文本...
        /// </summary>
        public static void SetTextWithEllipsis(this Text compontent, string content)
        {
            TextGenerator generator = new TextGenerator();

            RectTransform rectTransform = compontent.GetComponent<RectTransform>();

            TextGenerationSettings settings = compontent.GetGenerationSettings(rectTransform.rect.size);

            generator.Populate(content, settings);

            int count = generator.characterCountVisible;

            var text = content;

            if (content.Length > count)
            {
                text = content.Substring(0, count - 3) + "...";
            }

            compontent.text = text;
        }
        /// <summary>
        /// 设置带省略号文本...
        /// </summary>
        public static void SetTextWithEllipsis(this TextMeshProUGUI compontent, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                compontent.text = string.Empty;
            }
            else
            {
                compontent.text = content;

                compontent.ForceMeshUpdate();

                string text = compontent.GetParsedText();

                if (string.IsNullOrEmpty(text)) return;

                if (text.Length < 3) return;

                if (text.Length < content.Length)
                {
                    text = content.Substring(0, text.Length - 3) + "...";
                }

                compontent.text = text;
            }
        }
        /// <summary>
        /// 设置文本横向显示区域
        /// </summary>
        public static float SetTextWithWidth(this Text compontent, string content, float space = 0, float min = -1, float max = -1)
        {
            compontent.text = content;

            float width = compontent.preferredWidth;

            width += space;

            if (min != -1) width = Math.Max(width, min);

            if (max != -1) width = Math.Min(width, max);

            return width;
        }
        /// <summary>
        /// 设置文本纵向显示区域
        /// </summary>
        public static float SetTextWithHeight(this Text compontent, string content, float space = 0, float min = -1, float max = -1)
        {
            compontent.text = content;

            float height = compontent.preferredHeight;

            height += space;

            if (min != -1) height = Math.Max(height, min);

            if (max != -1) height = Math.Min(height, max);

            return height;
        }
        /// <summary>
        /// 创建表情
        /// </summary>
        private static void CreateImageEmoji(EmojiInformation emoji, Transform parent)
        {
            if (emoji == null) return;

            RectTransform go = new GameObject("emoji").AddComponent<RectTransform>();

            Image image = go.gameObject.AddComponent<Image>();

            SpriteAtlas atlas = Resources.Load<SpriteAtlas>("emoji");

            go.SetParent(parent);

            go.pivot = new Vector2(0.55f, 1);

            go.anchoredPosition = emoji.position;

            go.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, emoji.size.x);

            go.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, emoji.size.y);

            image.sprite = atlas.GetSprite(emoji.name);
        }

        class EmojiInformation
        {
            public string name;

            public int index;

            public Vector2 position;

            public Vector2 size;
        }
    }
}