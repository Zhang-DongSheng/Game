using System.Collections;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollDirection : MonoBehaviour
    {
        [SerializeField] private ScrollRect scroll;

        private float spacing;

        private float position;

        private void Awake()
        {
            if (scroll.content.TryGetComponent(out HorizontalLayoutGroup horizontal))
            {
                spacing = horizontal.spacing;
            }
            else if (scroll.content.TryGetComponent(out VerticalLayoutGroup vertical))
            {
                spacing = vertical.spacing;
            }
            else if (scroll.content.TryGetComponent(out GridLayoutGroup grid))
            {
                spacing = grid.spacing.x;
            }
        }

        public void Direction(int index)
        {
            int count = scroll.content.childCount;

            if (index >= count) return;

            StartCoroutine(DirectionPosition(index));
        }

        private IEnumerator DirectionPosition(int index)
        {
            yield return new WaitForEndOfFrame();

            position = 0;

            if (index > 0)
            {
                for (int i = 0; i < index; i++)
                {
                    if (scroll.content.GetChild(i).GetComponent<RectTransform>().rect.width > 0)
                    {
                        position -= scroll.content.GetChild(i).GetComponent<RectTransform>().rect.width + spacing;
                    }
                }
                position -= scroll.content.GetChild(index).GetComponent<RectTransform>().rect.width * 0.5f;

                position -= spacing;

                position += scroll.viewport.rect.width * 0.5f;

                if (Mathf.Abs(position) > scroll.content.rect.width - scroll.viewport.rect.width)
                {
                    position = (scroll.content.rect.width - scroll.viewport.rect.width) * -1f;
                }
                position = Mathf.Min(0, position);
            }
            scroll.content.anchoredPosition = new Vector2(position, 0);
        }
    }
}