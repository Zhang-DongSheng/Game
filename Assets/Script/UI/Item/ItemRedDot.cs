using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIRedDot : MonoBehaviour
    {
        [SerializeField] private GameObject target;

        [SerializeField] private RedKey key;

        [SerializeField] private List<RedKey> list;

        private bool active;

        private void Awake()
        {
            EventManager.RegisterEvent(EventKey.RedDot, Refresh);
        }

        private void Start()
        {
            RefreshRedDot();
        }

        private void Refresh(EventMessageArgs args)
        {
            RefreshRedDot();
        }

        private void RefreshRedDot()
        {
            active = ReddotLogic.Instance.Trigger(key) || ReddotLogic.Instance.Trigger(list.ToArray());

            SetActive(active);
        }

        public void UpdeteRedDotKey(params RedKey[] keys)
        {
            key = RedKey.None; list.Clear();

            switch (keys.Length)
            {
                case 0:
                    break;
                case 1:
                    key = keys[0];
                    break;
                default:
                    list.AddRange(keys);
                    break;
            }
            RefreshRedDot();
        }

        private void SetActive(bool active)
        {
            if (target != null && target.activeSelf != active)
            {
                target.SetActive(active);
            }
        }

        private void OnDestroy()
        {
            EventManager.UnregisterEvent(EventKey.RedDot, Refresh);
        }
    }
}