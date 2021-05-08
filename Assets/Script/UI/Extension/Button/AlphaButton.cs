using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// Í¸Ã÷°´Å¥
    /// </summary>
    public class AlphaButton : Image, IPointerClickHandler
    {
        [SerializeField, Range(0, 1f)] private float alpha = 0.1f;

        [Space(5)] public UnityEvent onClick;

        protected override void Awake()
        {
            base.Awake();

            base.alphaHitTestMinimumThreshold = alpha;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }
    }
}