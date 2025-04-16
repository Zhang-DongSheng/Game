using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ClubView : ViewBase
    {
        [SerializeField] private RawImage image;

        [SerializeField] private string key;

        private void Awake()
        {
            CameraSnapshot.Instance.GetTexture(key, (texture)=>
            {
                image.texture = texture;
            });
        }
    }
}