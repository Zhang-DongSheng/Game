using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    /// <summary>
    /// 竖向文本[有Bug]
    /// </summary>
    public class TextVertical : Text
    {
        private float lineSpace = 1;

        private float textSpace = 1;

        private float xOffset = 0;

        private float yOffset = 0;

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (!isActiveAndEnabled) return;

            lineSpace = fontSize * lineSpacing;

            textSpace = fontSize * lineSpacing;

            xOffset = rectTransform.sizeDelta.x * 0.5f - fontSize * 0.5f;

            yOffset = rectTransform.sizeDelta.y * 0.5f - fontSize * 0.5f;

            base.OnPopulateMesh(toFill);

            int count = cachedTextGenerator.lines.Count;

            int last = count - 1;

            for (int i = 0; i < count; i++)
            {
                UILineInfo line = cachedTextGenerator.lines[i];

                if (i == last)
                {
                    int current = 0;

                    for (int j = line.startCharIdx; j < cachedTextGenerator.characterCountVisible; j++)
                    {
                        modifyText(toFill, j, current++, i);
                    }
                }
                else
                {
                    UILineInfo next = cachedTextGenerator.lines[i + 1];

                    int current = 0;

                    for (int j = line.startCharIdx; j < next.startCharIdx - 1; j++)
                    {
                        modifyText(toFill, j, current++, i);
                    }
                }
            }
        }

        void modifyText(VertexHelper helper, int i, int charYPos, int charXPos)
        {
            UIVertex lb = new UIVertex();
            helper.PopulateUIVertex(ref lb, i * 4);

            UIVertex lt = new UIVertex();
            helper.PopulateUIVertex(ref lt, i * 4 + 1);

            UIVertex rt = new UIVertex();
            helper.PopulateUIVertex(ref rt, i * 4 + 2);

            UIVertex rb = new UIVertex();
            helper.PopulateUIVertex(ref rb, i * 4 + 3);

            Vector3 center = Vector3.Lerp(lb.position, rt.position, 0.5f);
            Matrix4x4 move = Matrix4x4.TRS(-center, Quaternion.identity, Vector3.one);

            float x = -charXPos * lineSpace + xOffset;
            float y = -charYPos * textSpace + yOffset;

            Vector3 pos = new Vector3(x, y, 0);
            Matrix4x4 place = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
            Matrix4x4 transform = place * move;

            lb.position = transform.MultiplyPoint(lb.position);
            lt.position = transform.MultiplyPoint(lt.position);
            rt.position = transform.MultiplyPoint(rt.position);
            rb.position = transform.MultiplyPoint(rb.position);

            helper.SetUIVertex(lb, i * 4);
            helper.SetUIVertex(lt, i * 4 + 1);
            helper.SetUIVertex(rt, i * 4 + 2);
            helper.SetUIVertex(rb, i * 4 + 3);
        }
    }
}