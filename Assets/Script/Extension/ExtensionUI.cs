using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static partial class Extension
{
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

    public static float SetTextAndWidth(this Text compontent, string content, float space = 0, float min = -1, float max = -1)
    {
        compontent.text = content;

        float width = compontent.preferredWidth;

        width += space;

        if (min != -1) width = Math.Max(width, min);

        if (max != -1) width = Math.Min(width, max);

        return width;
    }

    public static float SetTextAndHeight(this Text compontent, string content, float space = 0, float min = -1, float max = -1)
    {
        compontent.text = content;

        float height = compontent.preferredHeight;

        height += space;

        if (min != -1) height = Math.Max(height, min);

        if (max != -1) height = Math.Min(height, max);

        return height;
    }

    public static void ToTop(this ScrollRect scroll)
    {
        scroll.normalizedPosition = new Vector2(0, 1);
    }

    public static void ToBottom(this ScrollRect scroll)
    {
        scroll.normalizedPosition = new Vector2(0, 0);
    }
}