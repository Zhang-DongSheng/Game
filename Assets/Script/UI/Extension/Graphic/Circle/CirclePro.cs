using System.Collections.Generic;

namespace UnityEngine.UI
{
    /// <summary>
    /// 圆形专业版
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class CirclePro : MaskableGraphic
    {
        [SerializeField] private Texture2D texture;

        [SerializeField] private float top;

        [SerializeField] private float left;

        [SerializeField] private float right;

        [SerializeField] private float bottom;

        [SerializeField, Range(1, 180)] private int segements = 90;

        private Vector2 position, offset, space = new Vector2();

        private float radian;

        private int step, count;

        private readonly List<Vector2> centre = new List<Vector2>(4)
        {
            new Vector2(0, 0), new Vector2(0, 0),
            new Vector2(0, 0), new Vector2(0, 0)
        };

        public override Texture mainTexture => texture == null ? s_WhiteTexture : texture;

        protected override void OnPopulateMesh(VertexHelper helper)
        {
            Ready();

            helper.Clear();

            step = 4; float value = 0;

            helper.AddVert(GetUIVertex(centre[0]));

            helper.AddVert(GetUIVertex(centre[1]));

            helper.AddVert(GetUIVertex(centre[2]));

            helper.AddVert(GetUIVertex(centre[3]));

            #region 矩形
            if (space.x - right > left)
            {
                helper.AddVert(GetUIVertex(new Vector2(left, 0) - offset));

                helper.AddVert(GetUIVertex(new Vector2(space.x - right, 0) - offset));

                helper.AddVert(GetUIVertex(new Vector2(space.x - right, space.y) - offset));

                helper.AddVert(GetUIVertex(new Vector2(left, space.y) - offset));

                helper.AddTriangle(step + 1, step, step + 2);

                helper.AddTriangle(step + 2, step, step + 3);

                step += 4;
            }

            if (space.y - top > bottom)
            {
                helper.AddVert(GetUIVertex(new Vector2(0, bottom) - offset));

                helper.AddVert(GetUIVertex(new Vector2(space.x, bottom) - offset));

                helper.AddVert(GetUIVertex(new Vector2(space.x, space.y - top) - offset));

                helper.AddVert(GetUIVertex(new Vector2(0, space.y - top) - offset));

                helper.AddTriangle(step + 1, step, step + 2);

                helper.AddTriangle(step + 2, step, step + 3);

                step += 4;
            }
            #endregion

            #region 四角半圆
            AddSemiangle(helper, 0, right, top, ref value);

            AddSemiangle(helper, 1, left, top, ref value);

            AddSemiangle(helper, 2, left, bottom, ref value);

            AddSemiangle(helper, 3, right, bottom, ref value);
            #endregion
        }

        private void Ready()
        {
            radian = 0.5f * Mathf.PI / segements;

            space.x = rectTransform.rect.width;

            space.y = rectTransform.rect.height;

            offset = space * 0.5f;

            count = segements + 1;

            centre[0] = new Vector2(space.x - right, space.y - top) - offset;

            centre[1] = new Vector2(left, space.y - top) - offset;

            centre[2] = new Vector2(left, bottom) - offset;

            centre[3] = new Vector2(space.x - right, bottom) - offset;
        }

        private void AddSemiangle(VertexHelper helper, int index, float width, float height, ref float value)
        {
            if (width > 0 && height > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    position.x = centre[index].x + Mathf.Cos(value) * width;

                    position.y = centre[index].y + Mathf.Sin(value) * height;

                    if (i < segements)
                    {
                        value += radian;
                    }
                    helper.AddVert(GetUIVertex(position));
                }

                for (int i = 0; i < count; i++)
                {
                    if (i < segements)
                    {
                        helper.AddTriangle(step + i, index, step + i + 1);
                    }
                }

                step += count;
            }
            else
            {
                value += radian * segements;
            }
        }

        private UIVertex GetUIVertex(Vector2 position)
        {
            return new UIVertex()
            {
                position = position,
                color = color,
                uv0 = position / space + Vector2.one * 0.5f,
            };
        }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }
    }
}