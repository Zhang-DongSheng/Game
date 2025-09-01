using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [DisallowMultipleComponent, RequireComponent(typeof(Image))]
    public class ImageBind : MonoBehaviour
    {
        [SerializeField] private string content;

        private Image _image;

        private bool _relevance = false;

        private void Awake()
        {
            SetSpriteImmediately(content);
        }

        private void OnValidate()
        {
            SetSprite(content);
        }

        private void Relevance()
        {
            if (_relevance) return;

            _relevance = true;

            _image = GetComponent<Image>();
        }

        public void SetColor(Color color)
        {
            Relevance();
            if (_image != null)
                _image.color = color;
        }

        public void SetSprite(string content)
        {
            if (this.content.Equals(content)) return;

            this.content = content;

            SetSpriteImmediately(content);
        }

        protected void SetSprite(Sprite sprite)
        {
            Relevance();
            if (_image != null)
                _image.sprite = sprite;
        }

        protected void SetSpriteImmediately(string content)
        {
            var sprite = SpriteManager.Instance.GetSprite(content);

            SetSprite(sprite);
        }
    }
}