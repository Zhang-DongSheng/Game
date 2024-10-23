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
            SetTextImmediately(content);
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

        public void SetSprite(string content)
        {
            if (this.content.Equals(content)) return;

            this.content = content;

            SetTextImmediately(content);
        }

        public void SetTextImmediately(string content)
        {
            var sprite = SpriteManager.Instance.GetSprite(content);

            SetSprite(sprite);
        }

        protected void SetSprite(Sprite sprite)
        {
            Relevance();
            if (_image != null)
                _image.sprite = sprite;
        }
    }
}