using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        public static void SetPosition(this Transform target, Axis axis, float value, bool local = true)
        {
            Vector3 position = local ? target.localPosition : target.position;

            switch (axis)
            {
                case Axis.X:
                    position.x = value;
                    break;
                case Axis.Y:
                    position.y = value;
                    break;
                case Axis.Z:
                    position.z = value;
                    break;
            }

            if (local)
            {
                target.localPosition = position;
            }
            else
            {
                target.position = position;
            }
        }

        public static void SetRotation(this Transform target, Axis axis, float value, bool local = true)
        {
            Vector3 eulerAngles = local ? target.localEulerAngles : target.eulerAngles;

            switch (axis)
            {
                case Axis.X:
                    eulerAngles.x = value;
                    break;
                case Axis.Y:
                    eulerAngles.y = value;
                    break;
                case Axis.Z:
                    eulerAngles.z = value;
                    break;
            }

            if (local)
            {
                target.localEulerAngles = eulerAngles;
            }
            else
            {
                target.eulerAngles = eulerAngles;
            }
        }

        public static void SetScale(this Transform target, Axis axis, float value)
        {
            Vector3 scale = target.localScale;

            switch (axis)
            {
                case Axis.X:
                    scale.x = value;
                    break;
                case Axis.Y:
                    scale.y = value;
                    break;
                case Axis.Z:
                    scale.z = value;
                    break;
            }
            target.localScale = scale;
        }

        public static void Clear(this Transform target)
        {
            if (target != null && target.childCount > 0)
            {
                for (int i = target.childCount - 1; i > -1; i--)
                {
                    GameObject.Destroy(target.GetChild(i).gameObject);
                }
            }
        }
    }

    public enum Axis
    {
        X,
        Y,
        Z,
    }
}