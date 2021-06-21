using System;
using UnityEngine;
using UnityEngine.Factory;

namespace Game.UI
{
    public class UICtrlBase
    {
        private UIBase view;

        private object[] paramter;

        private int number;

        private bool active;

        private UIStatus status = UIStatus.None;

        public void Open(UIPanel key, UILayer layer)
        {
            active = true; number++;

            switch (status)
            {
                case UIStatus.None:
                    Load(key, layer);
                    break;
                case UIStatus.Done:
                    Show();
                    break;
            }
        }

        public void Paramter(params object[] paramter)
        {
            this.paramter = paramter;
        }

        public void Close(bool destroy)
        {
            active = false;

            if (destroy)
            {
                if (view != null && view.gameObject != null)
                {
                    GameObject.Destroy(view.gameObject);
                }
            }
            else
            {
                Hide();
            }
        }

        protected virtual void Show()
        {
            if (view != null)
            {
                view.Refresh(paramter); view.Reopen();

                UIManager.Instance.SortDisplay(view.layer, view.transform);
            }
        }

        protected virtual void Hide()
        {
            if (view != null)
            {
                view.SetActive(false);
            }
        }

        private void Load(UIPanel key, UILayer layer)
        {
            status = UIStatus.Loading;

            try
            {
                Create(key, layer, Factory.Instance.Pop(key.ToString()) as GameObject);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void LoadAsync(UIPanel key, UILayer layer)
        {
            status = UIStatus.Loading;
        }

        private void Create(UIPanel key, UILayer layer, GameObject go)
        {
            RectTransform rect = go.GetComponent<RectTransform>();

            view = go.GetComponent<UIBase>();

            view.layer = layer != UILayer.None ? layer : view.layer;

            view.SetParent(UIManager.Instance.GetParent(view.layer));

            view.SetName(key.ToString());

            rect.Reset(); rect.Full();

            view.Init();

            view.Refresh(paramter);

            view.SetActive(active);

            UIManager.Instance.SortDisplay(view.layer, go.transform);

            status = UIStatus.Done;
        }

        public int Number { get { return number; } }

        enum UIStatus
        {
            None,
            Loading,
            Done,
        }
    }
}