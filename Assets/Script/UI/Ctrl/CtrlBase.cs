using System;
using UnityEngine;
using UnityEngine.Factory;

namespace Game.UI
{
    public class CtrlBase
    {
        protected UIBase view;

        protected Paramter paramter;

        protected int number;

        protected bool active;

        private Status status = Status.None;

        public void Open(UIPanel key, UILayer layer)
        {
            active = true; number++;

            switch (status)
            {
                case Status.None:
                    Load(key, layer);
                    break;
                case Status.Loading:
                    Debug.LogWarningFormat("The paenl of [{0}] is loading...", key);
                    break;
                case Status.Display:
                    Show();
                    break;
            }
        }

        public void Paramter(Paramter paramter)
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
                status = Status.None;
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
                view.Reopen(); view.Refresh(paramter);

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
            status = Status.Loading;

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
            status = Status.Loading;
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

            status = Status.Display;
        }

        public int Number { get { return number; } }

        enum Status
        {
            None,
            Loading,
            Display,
        }
    }
}