using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIBubble : UIBase
    {
        [SerializeField] private RectTransform background;

        [SerializeField] private RectTransform content;

        [SerializeField] private Text txtValue;

        [SerializeField] private Button close;

        protected override void OnAwake()
        {
            close.onClick.AddListener(OnClickClose);
        }

        public override void Refresh(UIParameter parameter)
        {
            var target = parameter.Get<Transform>("transform");

            var value = parameter.Get<string>("value");

            txtValue.text = value;

            FixedPosition(target.position);
        }

        private void FixedPosition(Vector3 world)
        {
            var camera = UIManager.Instance.Canvas.worldCamera;

            var point = RectTransformUtility.WorldToScreenPoint(camera, world);

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, point, camera, out Vector2 position))
            {

            }
            // Æ«ÒÆ
            var offset = new Vector2(0, content.rect.height * 0.5f + 50);

            if (position.y > 0)
            {
                position -= offset;
            }
            else
            {
                position += offset;
            }
            content.AdjustPosition(position, background);
        }
    }
}