using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class ImageBind : MonoBehaviour
    {
        public string content;

        private Image m_image;

        private void Awake()
        {
            SetSprite(content);
        }

        public void SetSprite(string content, bool native = false)
        {
            if (this.content.Equals(content)) return;

            this.content = content;

            if (m_image == null)
                m_image = GetComponent<Image>();

            m_image.sprite = SpriteManager.Instance.GetSprite(content);

            if (native)
            {
                m_image.SetNativeSize();
            }
        }
    }
}