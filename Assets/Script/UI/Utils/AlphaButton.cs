using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// 透明按钮
    /// </summary>
    /// Image使用的图片需要开启[read write enable]属性
    /// 图片在内存中的大小将变成原先的两倍
    /// 图片不能打进UGUI图集里面
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