using System;
using System.Collections;
using UnityEngine;

namespace Game.UI
{
    public abstract class UIBase : MonoBehaviour
    {
        public UILayer layer = UILayer.Base;

        private int ID;

        public virtual void Init() { }

        public virtual void Reopen()
        {
            SetActive(true);
        }

        public virtual void Refresh() { }

        public void Delay(float time, Action callBack = null)
        {
            StartCoroutine(DelayedExecution(time, callBack));
        }

        private IEnumerator DelayedExecution(float time, Action callBack)
        {
            yield return new WaitForSeconds(time);

            callBack?.Invoke();
        }

        public void First()
        {
            if (transform.parent != null)
            {
                transform.SetSiblingIndex(transform.parent.childCount - 1);
            }
        }

        public void SetID(int ID)
        {
            this.ID = ID;
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

        public void Reset()
        {

        }

        public virtual void Close()
        {
            SetActive(false);
        }
    }
}