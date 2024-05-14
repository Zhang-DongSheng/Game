using UnityEngine.EventSystems;

namespace Game.UI
{
    /// <summary>
    /// 关闭按钮
    /// </summary>
    public class ItemClose : ItemBase, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            var view = GetComponentInParent<ViewBase>();

            view.Back();
        }
    }
}