using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// ͸����ť
    /// </summary>
    /// Imageʹ�õ�ͼƬ��Ҫ����[read write enable]����
    /// ͼƬ���ڴ��еĴ�С�����ԭ�ȵ�����
    /// ͼƬ���ܴ��UGUIͼ������
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