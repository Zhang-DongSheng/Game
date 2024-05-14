using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 引用关联
    /// </summary>
    public class ItemRelevance : ItemBase
    {
        public Transform[] relevances;

        public Transform Get(string name)
        {
            foreach (var cell in relevances)
            {
                if (cell.name == name)
                {
                    return cell;
                }
            }
            return default;
        }

        public T Get<T>(string name) where T : Component
        {
            foreach (var cell in relevances)
            {
                if (cell.name == name)
                {
                    return cell.GetComponent<T>();
                }
            }
            return default;
        }

        public bool TryGet<T>(string name, out T component) where T : Component
        {
            component = default;

            foreach (var cell in relevances)
            {
                if (cell.name == name)
                {
                    return cell.TryGetComponent(out component);
                }
            }
            return false;
        }
    }
}