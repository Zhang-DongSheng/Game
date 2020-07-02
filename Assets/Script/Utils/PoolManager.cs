using System.Collections.Generic;

namespace UnityEngine
{
    [DisallowMultipleComponent]
    public class PoolManager : MonoBehaviour
    {
        private Transform pool_parent_UI;
        private Transform pool_parent_Object;

        private readonly Dictionary<string, Stack<GameObject>> m_pool = new Dictionary<string, Stack<GameObject>>();

        private void Awake()
        {
            pool_parent_Object = new GameObject("Pool_Object").transform;
            pool_parent_Object.SetParent(transform);

            GameObject go_ui = new GameObject("Pool_UI");
            Canvas canvas = go_ui.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasGroup canvasGroup = go_ui.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            //canvasGroup.interactable = false;
            pool_parent_UI = go_ui.transform;
            pool_parent_UI.SetParent(transform);
        }

        public void Push(string key, GameObject obj)
        {
            Store(obj);

            if (m_pool.ContainsKey(key))
            {
                m_pool[key].Push(obj);
            }
            else
            {
                Stack<GameObject> _stack = new Stack<GameObject>();
                _stack.Push(obj);
                m_pool.Add(key, _stack);
            }
        }

        public GameObject Pop(string key)
        {
            if (m_pool.ContainsKey(key))
            {
                if (m_pool[key].Count > 0)
                {
                    GameObject obj = m_pool[key].Pop();
                    PutOut(obj);
                    return obj;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void Store(GameObject obj)
        {
            if (obj != null)
            {
                if (obj.transform is RectTransform)
                {
                    obj.transform.SetParent(pool_parent_UI);
                }
                else
                {
                    obj.transform.SetParent(pool_parent_Object);
                }

                if (obj.activeSelf)
                {
                    obj.SetActive(false);
                }
            }
        }

        private void PutOut(GameObject obj)
        {
            if (obj != null && !obj.activeSelf)
            {
                obj.SetActive(true);
            }
        }

        public void Clear()
        {
            foreach (KeyValuePair<string, Stack<GameObject>> item in m_pool)
            {
                while (item.Value.Count != 0)
                {
                    Destroy(item.Value.Pop());
                }
            }
            m_pool.Clear();
        }
    }
}