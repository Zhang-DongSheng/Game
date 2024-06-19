using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// Í·Ïñ×éºÏ
    /// </summary>
    public class ItemAvatar : ItemBase
    {
        [SerializeField] private ImageBind head;

        [SerializeField] private ImageBind frame;

        public void Refresh(int head, int frame)
        {
            this.head.SetSprite("");

            this.frame.SetSprite("");
        }
    }
}