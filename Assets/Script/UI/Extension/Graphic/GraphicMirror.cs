using System.Collections.Generic;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Graphic))]
    public class GraphicMirror : BaseMeshEffect
    {
        [SerializeField] private Direction direction = Direction.Horizontal;

        private readonly List<UIVertex> vertexs = new List<UIVertex>();

        /// <summary>
        /// 设置原始尺寸
        /// </summary>
        [ContextMenu("SetNativeSize")]
        public void SetNativeSize()
        {
            if (graphic != null && graphic is Image)
            {
                Sprite overrideSprite = (graphic as Image).overrideSprite;

                if (overrideSprite != null)
                {
                    RectTransform rect = GetComponent<RectTransform>();

                    float w = overrideSprite.rect.width / (graphic as Image).pixelsPerUnit;

                    float h = overrideSprite.rect.height / (graphic as Image).pixelsPerUnit;

                    rect.anchorMax = rect.anchorMin;

                    switch (direction)
                    {
                        case Direction.None:
                            rect.sizeDelta = new Vector2(w, h);
                            break;
                        case Direction.Horizontal:
                            rect.sizeDelta = new Vector2(w * 2, h);
                            break;
                        case Direction.Vertical:
                            rect.sizeDelta = new Vector2(w, h * 2);
                            break;
                        case Direction.Slant:
                            rect.sizeDelta = new Vector2(w * 2, h * 2);
                            break;
                    }
                    graphic.SetVerticesDirty();
                }
            }
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive()) return;

            vh.GetUIVertexStream(vertexs);

            int count = vertexs.Count;

            if (graphic is Image)
            {
                Image.Type type = (graphic as Image).type;

                switch (type)
                {
                    case Image.Type.Simple:
                        DrawSimple(vertexs, count);
                        break;
                    case Image.Type.Sliced:

                        break;
                    case Image.Type.Tiled:

                        break;
                    case Image.Type.Filled:

                        break;
                }
            }
            else
            {
                DrawSimple(vertexs, count);
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(vertexs);
        }

        /// <summary>
        /// 绘制简单版
        /// </summary>
        /// <param name="output"></param>
        /// <param name="count"></param>
        protected void DrawSimple(List<UIVertex> output, int count)
        {
            Rect rect = graphic.GetPixelAdjustedRect();

            SimpleScale(rect, output, count);

            switch (direction)
            {
                case Direction.Horizontal:
                    ExtendCapacity(output, count);
                    MirrorVerts(rect, output, count, true);
                    break;
                case Direction.Vertical:
                    ExtendCapacity(output, count);
                    MirrorVerts(rect, output, count, false);
                    break;
                case Direction.Slant:
                    ExtendCapacity(output, count * 3);
                    MirrorVerts(rect, output, count, true);
                    MirrorVerts(rect, output, count * 2, false);
                    break;
            }
        }

        protected void ExtendCapacity(List<UIVertex> verts, int addCount)
        {
            var neededCapacity = verts.Count + addCount;
            if (verts.Capacity < neededCapacity)
            {
                verts.Capacity = neededCapacity;
            }
        }

        protected void SimpleScale(Rect rect, List<UIVertex> verts, int count)
        {
            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = verts[i];

                Vector3 position = vertex.position;

                if (direction == Direction.Horizontal || direction == Direction.Slant)
                {
                    position.x = (position.x + rect.x) * 0.5f;
                }

                if (direction == Direction.Vertical || direction == Direction.Slant)
                {
                    position.y = (position.y + rect.y) * 0.5f;
                }

                vertex.position = position;

                verts[i] = vertex;
            }
        }

        protected void MirrorVerts(Rect rect, List<UIVertex> verts, int count, bool isHorizontal = true)
        {
            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = verts[i];

                Vector3 position = vertex.position;

                if (isHorizontal)
                {
                    position.x = rect.center.x * 2 - position.x;
                }
                else
                {
                    position.y = rect.center.y * 2 - position.y;
                }

                vertex.position = position;

                verts.Add(vertex);
            }
        }
    }
}