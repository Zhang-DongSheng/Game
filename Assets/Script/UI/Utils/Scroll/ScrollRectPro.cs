using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class ScrollRectPro : ScrollRect, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private ScrollRect multiScroll;

        private ScrollCtrl ctrl;

        protected override void Awake()
        {
            base.Awake();

            if (multiScroll == null)
                multiScroll = GetMultiScroll();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (horizontal)
            {
                ctrl = ScrollUtils.Horizontal(eventData.delta) ? ScrollCtrl.Single : ScrollCtrl.Multi;
            }
            else if (vertical)
            {
                ctrl = ScrollUtils.Vertical(eventData.delta) ? ScrollCtrl.Single : ScrollCtrl.Multi;
            }

            if (ctrl == ScrollCtrl.Multi && multiScroll != null)
            {
                multiScroll.OnBeginDrag(eventData);
            }
            else
            {
                base.OnBeginDrag(eventData);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (ctrl == ScrollCtrl.Multi && multiScroll != null)
            {
                multiScroll.OnDrag(eventData);
            }
            else
            {
                base.OnDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (ctrl == ScrollCtrl.Multi && multiScroll != null)
            {
                multiScroll.OnEndDrag(eventData);
            }
            else
            {
                base.OnEndDrag(eventData);
            }
        }

        private ScrollRect GetMultiScroll()
        {
            ScrollRect[] scrolls = GetComponentsInParent<ScrollRect>();

            for (int i = 0; i < scrolls.Length; i++)
            {
                if (i == 1)
                {
                    return scrolls[i];
                }
            }
            return null;
        }
    }

    public class ScrollUtils
    {
        private static readonly float Min = 45f;

        private static readonly float Max = 135f;

        private static float angle;

        public static bool Horizontal(Vector2 vector)
        {
            angle = Vector2.Angle(vector, Vector2.up);

            return angle > Min && angle < Max;
        }

        public static bool Vertical(Vector2 vector)
        {
            angle = Vector2.Angle(vector, Vector2.up);

            return angle < Min || angle > Max;
        }
    }

    public enum ScrollCtrl
    {
        Single,
        Multi,
    }
}