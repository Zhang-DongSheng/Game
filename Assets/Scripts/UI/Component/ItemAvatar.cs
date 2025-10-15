using Game.Data;
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

        public void Refresh(uint headID, uint frameID)
        {
            var table = DataAvatar.Get(headID);

            var head = table != null ? table.icon : string.Empty;

            table = DataAvatar.Get(frameID);

            var frame = table != null ? table.icon : string.Empty;

            Refresh(head, frame);
        }

        public void Refresh(string head, string frame)
        {
            imgHead.SetSprite(head);

            imgFrame.SetSprite(frame);
        }
    }
}