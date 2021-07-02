using UnityEngine;

namespace Game
{
    public static partial class Extension
    {
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
}