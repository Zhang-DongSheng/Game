using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent, RequireComponent(typeof(Image))]
    public class ImageBind : MonoBehaviour
    {
        [SerializeField] private string content;

        private Image _image;

        private void Awake()
        {
            SetSprite(content);
        }

        private void OnValidate()
        {
            SetSprite(content);
        }

        public void SetSprite(string content)
        {
            if (this.content.Equals(content)) return;

            this.content = content;

            if (_image == null)
                _image = GetComponent<Image>();
            _image.sprite = SpriteHelper.Instance.GetSprite(content);
        }
    }
}