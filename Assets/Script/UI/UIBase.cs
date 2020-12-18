using System;
using System.Collections;

namespace UnityEngine.UI
{
    public abstract class UIBase : MonoBehaviour
    {
        [HideInInspector] public int panel_ID;
        [HideInInspector] public string panel_name;
        [HideInInspector] public PanelType panel_type;

        protected CanvasGroup panel_canvas;

        public virtual void Reopen()
        {
            SetActive(true);
        }

        public virtual void Close(Action callBack = null)
        {
            callBack?.Invoke();

            SetActive(false);
        }

        public virtual void Init() { }

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

        public void SetActive_Canvas(bool active)
        {
            if (panel_canvas != null)
            {
                panel_canvas.alpha = active ? 1 : 0;
                panel_canvas.interactable = active;
                panel_canvas.blocksRaycasts = active;
            }
        }

        public void SetParent(Transform parent)
        {
            if (transform.parent != parent)
            {
                transform.SetParent(parent);
            }
        }

        public override string ToString()
        {
            return panel_name;
        }
    }
}