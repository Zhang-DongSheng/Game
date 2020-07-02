using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class UIUtils
    {
        public static Vector3 PositionOffset(Transform target, int count = 3)
        {
            Vector3 position = Vector3.zero;

            while (target != null)
            {
                if (count-- > 0)
                {
                    position += target.localPosition;

                    target = target.parent;
                }
                else
                {
                    break;
                }
            }

            return position;
        }

        public static Vector3 PositionFormat(Vector3 position, Vector3 offset)
        {
            float ratio_width = 1, ratio_height = 1;

            if (UIConfig.ResolutionRatio != UIConfig.ScreenRatio)
            {
                if (UIConfig.ResolutionRatio < UIConfig.ScreenRatio)
                {
                    float real = Screen.height * UIConfig.ResolutionRatio;
                    ratio_width = Screen.width / real;
                }
                else
                {
                    float real = Screen.width / UIConfig.ResolutionRatio;
                    ratio_height = Screen.height / real;
                }
            }

            position.x -= UIConfig.ScreenHalfWidth;
            position.y -= UIConfig.ScreenHalfHeight;

            position.x *= UIConfig.ScreenWidthRatio * ratio_width;
            position.y *= UIConfig.ScreenHeightRatio * ratio_height;

            position -= offset;

            return position;
        }
    }
}