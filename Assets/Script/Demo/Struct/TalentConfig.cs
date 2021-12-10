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
        private static bool[] exists = new bool[4];

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

        public static TalentStatus Status(List<int> list, List<int> preview, int src, int dst = -1)
        {
            if (dst == -1)
            {
                if (list.Exists(x => x == src))
                {
                    return TalentStatus.Light;
                }
                else if (preview.Exists(x => x == src))
                {
                    return TalentStatus.Preview;
                }
                else
                {
                    return TalentStatus.None;
                }
            }
            else
            {
                exists[0] = list.Exists(x => x == src);

                exists[1] = list.Exists(x => x == dst);

                if (exists[0] && exists[1])
                {
                    return TalentStatus.Light;
                }
                else
                {
                    exists[2] = preview.Exists(x => x == src);

                    exists[3] = preview.Exists(x => x == dst);

                    if (exists[0] && exists[3])
                    {
                        return TalentStatus.Preview;
                    }
                    else if (exists[1] && exists[2])
                    {
                        return TalentStatus.Preview;
                    }
                    else if (exists[2] && exists[3])
                    {
                        return TalentStatus.Preview;
                    }
                    else
                    {
                        return TalentStatus.None;
                    }
                }
            }
        }

        public static List<int> Search(TalentSystem talent, int target, List<int> activated)
        {
            TalentQueue queue = talent.queues.Find(x => x.key == target);

            if (queue == null) return null;

            TalentPossible _possible, possible = null;

            int min = -1;

            int count = activated.Count;

            for (int i = 0; i < activated.Count; i++)
            {
                _possible = queue.possibles.Find(x => x.final == activated[i]);

                if (_possible != null)
                {
                    if (min == -1 || min > _possible.routes.Count)
                    {
                        min = _possible.routes.Count; possible = _possible;
                    }
                }
            }

            if (possible != null)
            {
                return new List<int>(possible.routes);
            }
            return null;
        }

        public static List<int> Search(TalentSystem talent, TalentSkill skill, List<int> activated)
        {
            bool exist = false;

            List<int> children = new List<int>();

            for (int i = 0; i < skill.children.Count; i++)
            {
                if (activated.Exists(x => x == skill.children[i].ID))
                {
                    exist = true;
                }
                else
                {
                    children.Add(skill.children[i].ID);
                }
            }

            if (exist) return children;

            List<int> _route, route = null;

            int min = -1;

            for (int i = 0; i < children.Count; i++)
            {
                _route = Search(talent, children[i], activated);

                if (min == -1 || min > _route.Count)
                {
                    min = _route.Count; route = _route;
                }
            }
            route.AddRange(children);

            return route;
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