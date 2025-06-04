using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIAudit : MonoBehaviour
    {
        private readonly Vector3[] corners = new Vector3[4];

        private void OnDrawGizmos()
        {
            var components = GetComponentsInChildren<MaskableGraphic>(true);

            foreach (var graphic in components)
            {
                if (graphic.raycastTarget)
                {
                    RectTransform rectTransform = graphic.transform as RectTransform;
                    rectTransform.GetWorldCorners(corners);
                    Gizmos.color = Color.blue;
                    for (int i = 0; i < 4; i++)
                        Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
                }
            }
        }
    }
}
