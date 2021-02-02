using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RawImage))]
    public class ItemPSD : MonoBehaviour
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
