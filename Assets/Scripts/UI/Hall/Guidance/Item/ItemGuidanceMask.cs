using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Image))]
    public class ItemGuidanceMask : ItemGuidanceBase
    {
        public TextAnchor anchor;

        public Vector2 position;

        public Vector2 size = new Vector2(100, 100);

        public Material material;

        private void Awake()
        {
            if (material == null) return;

            GetComponent<Image>().material = new Material(material);
        }

        private void OnValidate()
        {
            Refresh();
        }

        private void Refresh()
        {
            var material = GetComponent<Image>().material;

            var vector = new Vector4(position.x, position.y, size.x, size.y);

            material.SetVector("_Area", vector);
        }

        public void SetPostion(Vector2 position)
        {
            this.position = position;

            Refresh();
        }

        public void SetSize(Vector2 size)
        {
            this.size = size;

            Refresh();
        }

        public void SetArea(Vector2 position, Vector2 size)
        {
            this.position = position;

            this.size = size;

            Refresh();
        }
    }
}
