using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        static private Vector3 position;

        static private Vector3 rotation;

        static private Vector3 scale;
        /// <summary>
        /// 设置位置
        /// </summary>
        public static void SetPosition(this Transform transform, Axis axis, float value, bool local = true)
        {
            position = local ? transform.localPosition : transform.position;

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
                transform.localPosition = position;
            }
            else
            {
                transform.position = position;
            }
        }
        /// <summary>
        /// 设置方向
        /// </summary>
        public static void SetRotation(this Transform transform, Axis axis, float value, bool local = true)
        {
            rotation = local ? transform.localEulerAngles : transform.eulerAngles;

            switch (axis)
            {
                case Axis.X:
                    rotation.x = value;
                    break;
                case Axis.Y:
                    rotation.y = value;
                    break;
                case Axis.Z:
                    rotation.z = value;
                    break;
            }

            if (local)
            {
                transform.localEulerAngles = rotation;
            }
            else
            {
                transform.eulerAngles = rotation;
            }
        }
        /// <summary>
        /// 设置大小
        /// </summary>
        public static void SetScale(this Transform transform, Axis axis, float value)
        {
            scale = transform.localScale;

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
            transform.localScale = scale;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Default(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        /// <summary>
        /// 清除子节点
        /// </summary>
        public static void Clear(this Transform transform)
        {
            if (transform != null && transform.childCount > 0)
            {
                for (int i = transform.childCount - 1; i > -1; i--)
                {
                    GameObject.Destroy(transform.GetChild(i).gameObject);
                }
            }
        }
        /// <summary>
        /// 查早对象
        /// </summary>
        public static Transform FindByName(this Transform transform, string name, bool ignore = true)
        {
            Transform result = null;

            if (ignore)
            {
                var children = transform.GetComponentsInChildren<Transform>();

                int count = children.Length;

                for (int i = 0; i < count; i++)
                {
                    if (children[i].name == name)
                    {
                        result = children[i];
                        break;
                    }
                }
            }
            else
            {
                int count = transform.childCount;

                for (int i = 0; i < count; i++)
                {
                    Transform child = transform.GetChild(i);

                    if (child.name == name)
                    {
                        result = child;
                    }
                    else if (child.childCount > 0)
                    {
                        result = FindByName(child, name);
                    }
                    if (result != null) break;
                }
            }
            return result;
        }
    }

    public enum Axis
    {
        X,
        Y,
        Z,
    }
}