namespace UnityEngine.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class Circle : MaskableGraphic
    {
        [SerializeField] private Texture2D texture;

        [SerializeField, Range(3, 720)] private int segements = 360;

        private Vector2 position = new Vector2();

        private Vector2 space = new Vector2();

        public override Texture mainTexture => texture == null ? s_WhiteTexture : texture;

        protected override void OnPopulateMesh(VertexHelper helpr)
        {
            helpr.Clear();

            AddVert(helpr);

            AddTriangle(helpr);
        }

        private void AddVert(VertexHelper helper)
        {
            space.x = rectTransform.rect.width;

            space.y = rectTransform.rect.height;

            float radian = 2 * Mathf.PI / segements;

            helper.AddVert(GetUIVertex(color, Vector2.zero));

            float value = 0;

            for (int i = 0; i < segements; i++)
            {
                position.x = Mathf.Cos(value) * space.x * 0.5f;

                position.y = Mathf.Sin(value) * space.y * 0.5f;

                value += radian;

                helper.AddVert(GetUIVertex(color, position));
            }
        }

        private void AddTriangle(VertexHelper helper)
        {
            int index = 0;

            for (int i = 0; i < segements; i++)
            {
                index++;

                if (index == segements)
                {
                    helper.AddTriangle(index, 0, 1);
                }
                else
                {
                    helper.AddTriangle(index, 0, index + 1);
                }
            }
        }

        private UIVertex GetUIVertex(Color32 color, Vector3 position)
        {
            return new UIVertex()
            {
                position = position,
                color = color,
                uv0 = new Vector2(position.x / space.x + 0.5f, position.y / space.y + 0.5f),
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