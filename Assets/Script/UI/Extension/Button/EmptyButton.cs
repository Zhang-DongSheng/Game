using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// �հ�ť������Ⱦ
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class EmptyButton : Graphic, IPointerClickHandler
    {
        public UnityEvent onClick;

        protected EmptyButton()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }
    }
}