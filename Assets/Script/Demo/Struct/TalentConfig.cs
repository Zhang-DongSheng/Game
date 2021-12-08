using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TalentConfig
    {
        public const int CHILDREN = 6;

        public const int LAST = CHILDREN - 1;

        public const float DISTANCECIRCLE = 5f;

        public const float DISTANCESPACE = 100f;
    }

    public static class TalentUtils
    {
        public static void Link(TalentDirection direction, ref int src, ref int dst)
        {
            switch (direction)
            {
                case TalentDirection.TopLeft:
                    {
                        src = 5; dst = 2;
                    }
                    break;
                case TalentDirection.Left:
                    {
                        src = 0; dst = 3;
                    }
                    break;
                case TalentDirection.BottomLeft:
                    {
                        src = 1; dst = 4;
                    }
                    break;
                case TalentDirection.BottomRight:
                    {
                        src = 2; dst = 5;
                    }
                    break;
                case TalentDirection.Right:
                    {
                        src = 3; dst = 0;
                    }
                    break;
                case TalentDirection.TopRight:
                    {
                        src = 4; dst = 1;
                    }
                    break;
            }
        }

        public static float Rotation(TalentDirection direction)
        {
            switch (direction)
            {
                case TalentDirection.TopLeft:
                    return 60f;
                case TalentDirection.Left:
                    return 0f;
                case TalentDirection.BottomLeft:
                    return 300f;
                case TalentDirection.BottomRight:
                    return 240f;
                case TalentDirection.Right:
                    return 180f;
                case TalentDirection.TopRight:
                    return 120f;
                default:
                    return 0f;
            }
        }

        public static Vector2 Position(Vector2 position, TalentDirection direction, float distance = TalentConfig.DISTANCESPACE)
        {
            Vector2 offset = Vector2.zero;

            switch (direction)
            {
                case TalentDirection.TopLeft:
                    {
                        offset = new Vector2(.5f, 1);
                    }
                    break;
                case TalentDirection.Left:
                    {
                        offset = new Vector2(1, 0);
                    }
                    break;
                case TalentDirection.BottomLeft:
                    {
                        offset = new Vector2(.5f, -1);
                    }
                    break;
                case TalentDirection.BottomRight:
                    {
                        offset = new Vector2(-.5f, -1);
                    }
                    break;
                case TalentDirection.Right:
                    {
                        offset = new Vector2(-1, 0);
                    }
                    break;
                case TalentDirection.TopRight:
                    {
                        offset = new Vector2(-.5f, 1);
                    }
                    break;
            }
            return position + offset * distance;
        }

        public static Vector2 Position(int index, float distance = TalentConfig.DISTANCECIRCLE)
        {
            float angle = 360 - index * 60f;

            return new Vector2()
            {
                x = distance * Mathf.Cos(angle * Mathf.Deg2Rad),
                y = distance * Mathf.Sin(angle * Mathf.Deg2Rad)
            };
        }

        public static TalentStatus Status(List<int> list, int src, int dst = -1)
        {
            bool exist;

            if (dst == -1)
            {
                exist = list.Exists(x => x == src);
            }
            else
            {
                exist = list.Exists(x => x == src) && list.Exists(x => x == dst);
            }
            return exist ? TalentStatus.Light : TalentStatus.None;
        }
    }

    public enum TalentStatus
    {
        None,
        Preview,
        Light,
    }

    public enum TalentDirection
    {
        TopLeft,
        Left,
        BottomLeft,
        BottomRight,
        Right,
        TopRight,
    }
}