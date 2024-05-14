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

        public void Refresh(string head, string frame)
        {
            this.head.SetSprite(head);

            this.frame.SetSprite(frame);
        }
    }
}