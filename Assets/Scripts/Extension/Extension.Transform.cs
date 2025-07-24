using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
        private static Vector3 position;

        private static Vector3 rotation;

        private static Vector3 scale;
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
        public static void Reset(this Transform transform, float scale = 1)
        {
            transform.position = Vector3.zero;

            transform.rotation = Quaternion.identity;

            transform.localScale = Vector3.one * scale;
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
        /// 查找对象
        /// </summary>
        public static Transform FindByName(this Transform transform, string name)
        {
            Transform result = null;

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
            return result;
        }
        /// <summary>
        /// 找到目标并获取组件
        /// </summary>
        public static T FindComponent<T>(this Transform transform, string path) where T : Component
        {
            var target = transform.Find(path);

            if (target != null && target.TryGetComponent(out T component))
            {
                return component;
            }
            return default;
        }
        /// <summary>
        /// 找到目标并获取对象
        /// </summary>
        public static GameObject FindGameObject(this Transform transform, string path)
        {
            var target = transform.Find(path);

            if (target != null)
            {
                return target.gameObject;
            }
            return null;
        }
        /// <summary>
        /// 子节点数量
        /// </summary>
        public static int ChildrenCount(this Transform transform, bool includeInactive = false)
        {
            int count = 0;

            if (includeInactive)
            {
                count = transform.childCount;
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).gameObject.activeSelf)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        /// <summary>
        /// 目标全路径
        /// </summary>
        public static string FullName(this Transform transform)
        {
            string path = transform.name;

            Transform parent = transform.parent;

            while (parent != null)
            {
                path = string.Format("{0}/{1}", parent.name, path);

                parent = parent.parent;
            }
            return path;
        }
        /// <summary>
        /// 目标全路径
        /// </summary>
        public static string FullName(this Transform transform, Transform parent)
        {
            string path = transform.name;

            var node = transform.parent;

            while (node != null && node != parent)
            {
                path = string.Format("{0}/{1}", node.name, path);

                node = node.parent;
            }
            return path;
        }

        public static Vector3 WorldScale(this Transform transform)
        {
            Vector3 scale = transform.localScale;

            var parent = transform.parent;

            while (parent != null)
            {
                scale.x *= parent.localScale.x;
                scale.y *= parent.localScale.y;
                scale.z *= parent.localScale.z;
                parent = parent.parent;
            }
            return scale;
        }
    }
}