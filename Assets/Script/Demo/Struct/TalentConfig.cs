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
        public static int Index(TalentDirection direction, bool forward)
        {
            switch (direction)
            {
                case TalentDirection.TopLeft:
                    return forward ? 5 : 2;
                case TalentDirection.Left:
                    return forward ? 0 : 3;
                case TalentDirection.BottomLeft:
                    return forward ? 1 : 4;
                case TalentDirection.BottomRight:
                    return forward ? 2 : 5;
                case TalentDirection.Right:
                    return forward ? 3 : 0;
                case TalentDirection.TopRight:
                    return forward ? 4 : 1;
            }
            return 0;
        }

        public static Vector3Int Neighbours(TalentDirection direction, TalentSkill src, TalentSkill dst)
        {
            int indexSrc = Index(direction, true);

            int indexDst = Index(direction, false);

            return new Vector3Int(src.children[indexSrc].ID, dst.children[indexDst].ID, 0);
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

        public static List<int> Beeline(TalentSystem talent, int target, List<int> activated)
        {
            TalentQueue queue = talent.queues.Find(x => x.key == target);

            if (queue == null) return null;

            int min = -1, index = -1;

            int count = activated.Count;

            for (int i = 0; i < activated.Count; i++)
            {
                TalentPossible possible = queue.possibles.Find(x => x.final == activated[i]);

                if (possible != null)
                {
                    if (min == -1 || min > possible.routes.Count)
                    {
                        min = possible.routes.Count; index = i;
                    }
                }
            }

            if (index != -1)
            {
                return new List<int>(queue.possibles[index].routes);
            }
            return null;
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