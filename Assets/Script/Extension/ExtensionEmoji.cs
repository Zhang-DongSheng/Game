using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public static partial class Extension
{
    private const char special = '#';

    private static readonly Regex emojiRegex = new Regex(@"<emoji=(.+?)/>", RegexOptions.Singleline);

    private static readonly Regex specialRegex = new Regex(string.Format(@"X{0}", special), RegexOptions.Singleline);

    public class EmojiInformation
    {
        public string name;

        public int index;

        public Vector2 position;

        public Vector2 size;
    }

    public static void SetTextWithEmoji(this Text compontent, string content)
    {
        compontent.transform.ClearChildren();

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

    private static void CreateImageEmoji(EmojiInformation emoji, Transform parent)
    {
        if (emoji == null) return;

        RectTransform go = new GameObject("emoji").AddComponent<RectTransform>();

        Image image = go.AddComponent<Image>();

        SpriteAtlas atlas = Resources.Load<SpriteAtlas>("emoji");

        go.SetParent(parent);

        go.pivot = new Vector2(0.55f, 1);

        go.anchoredPosition = emoji.position;

        go.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, emoji.size.x);

        go.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, emoji.size.y);

        image.sprite = atlas.GetSprite(emoji.name);
    }
}