﻿using System;
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

    public static float SetTextAndGetWidth(this Text compontent, string content, float space, float min = 100, float max = 500)
    {
        compontent.text = content;

        float width = compontent.preferredWidth;

        width += space;

        width = Math.Max(width, min);

        width = Math.Min(width, max);

        return width;
    }
}