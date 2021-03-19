using System;
using System.Collections;
using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent]
    public abstract class UIBase : MonoBehaviour
    {
        public UILayer layer = UILayer.Base;

        public int order;

        public virtual void Init() { }

        public virtual void Reopen()
        {
            SetActive(true);
        }

        public virtual void Refresh(params object[] paramter) { }

        public void Delay(float time, Action callBack = null)
        {
            StartCoroutine(DelayedExecution(time, callBack));
        }

        private IEnumerator DelayedExecution(float time, Action callBack)
        {
            yield return new WaitForSeconds(time);

            callBack?.Invoke();
        }

        public void SetName(string name)
        {
            transform.name = name;
        }

        public void SetActive(bool active)
        {
            if (gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }

        public void SetParent(Transform parent)
        {
            if (transform.parent != parent)
            {
                transform.SetParent(parent);
            }
        }

        public virtual void Close()
        {
            SetActive(false);
        }
    }
}