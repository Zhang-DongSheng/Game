using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    /// <summary>
    /// Í·Ïñ×éºÏ
    /// </summary>
    public class ItemAvatar : ItemBase
    {
        [SerializeField] private ImageBind imgHead;

        [SerializeField] private ImageBind imgFrame;

        public UnityEvent onClick;

        private void Awake()
        {
            var button = GetComponent<UnityEngine.UI.Button>();

            button.onClick.AddListener(() => onClick?.Invoke());
        }

        public void Refresh(int head, int frame)
        {
            
        }

        public void Refresh(string head, string frame)
        {
            imgHead.SetSprite(head);

            imgFrame.SetSprite(frame);
        }
    }
}