using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(RawImage))]
    public class ItemPsd : MonoBehaviour
    {
        [SerializeField] private RectTransform target;

        [SerializeField] private RawImage image;

        public void Refresh(SpriteInformation sprite)
        {
            image.texture = sprite.texture;

            target.anchoredPosition = sprite.position;

            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sprite.size.x);
            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sprite.size.y);
        }

        public void SetActive(bool active)
        {
            if (gameObject != null && gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }
    }
}
