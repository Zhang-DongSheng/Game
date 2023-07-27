using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public static partial class Extension
    {
        public static void SetActive(this GameObject go, bool active, VisibleType visible)
        {
            if (go == null) return;

            switch (visible)
            {
                case VisibleType.Active:
                    {
                        if (go.activeSelf != active)
                        {
                            go.SetActive(active);
                        }
                    }
                    break;
                case VisibleType.Alpha:
                    {
                        if (go.TryGetComponent(out CanvasGroup canvas))
                        {
                            canvas.alpha = active ? 1 : 0;
                        }
                        else if (go.TryGetComponent(out Graphic graphic))
                        {
                            var color = graphic.color;

                            color.a = active ? 1 : 0;

                            graphic.color = color;
                        }
                        else if (go.TryGetComponent(out Renderer renderer))
                        {
                            var color = renderer.material.GetColor("_Color");

                            color.a = active ? 1 : 0;

                            renderer.material.SetColor("_Color", color);
                        }
                    }
                    break;
                case VisibleType.Cull:
                    {
                        if (go.TryGetComponent(out CanvasRenderer renderer))
                        {
                            renderer.cull = !active;
                        }
                    }
                    break;
                case VisibleType.Scale:
                    {
                        if (active)
                        {
                            go.transform.localScale = Vector3.one;
                        }
                        else
                        {
                            go.transform.localScale = Vector3.zero;
                        }
                    }
                    break;
            }
        }
    }
}