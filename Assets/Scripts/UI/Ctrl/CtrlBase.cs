using Game.Resource;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public class CtrlBase
    {
        protected UIBase view;

        protected Paramter paramter;

        private Status status;

        private bool async;

        private readonly Action<UIPanel, bool> callback;

        public CtrlBase(UIPanel panel, Action<UIPanel, bool> callback)
        {
            this.panel = panel;

            status = Status.None;

            this.callback = callback;
        }

        public void Open(UILayer layer, bool async)
        {
            this.layer = layer;

            this.async = async;

            Open();
        }

        public void Open()
        {
            active = true; number++;

            switch (status)
            {
                case Status.None:
                    {
                        if (async)
                            LoadAsync(panel, layer);
                        else
                            Load(panel, layer);
                    }
                    break;
                case Status.Loading:
                    Debuger.LogWarning(Author.UI, string.Format("The panel of [{0}] is loading...", panel));
                    break;
                case Status.Display:
                    {
                        if (view != null)
                        {
                            view.Reopen(); Show();
                        }
                    }
                    break;
                case Status.Error:
                    Debuger.LogError(Author.UI, string.Format("The panel of [{0}] resource was not found!", panel));
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
                status = Status.None; callback?.Invoke(panel, active);
            }
            else
            {
                Hide();
            }
        }

        private void Load(UIPanel panel, UILayer layer)
        {
            status = Status.Loading;

            try
            {
                Object prefab = ResourceManager.Load(string.Format("{0}/{1}.prefab", Config.Prefab, panel));

                Create(panel, layer, prefab);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void LoadAsync(UIPanel panel, UILayer layer)
        {
            status = Status.Loading;

            try
            {
                ResourceManager.LoadAsync(string.Format("{0}/{1}", Config.Prefab, panel), (asset) =>
                {
                    Create(panel, layer, asset);
                });
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Create(UIPanel panel, UILayer layer, Object prefab)
        {
            try
            {
                GameObject entity = GameObject.Instantiate(prefab) as GameObject;

                RectTransform rect = entity.GetComponent<RectTransform>();

                view = entity.GetComponent<UIBase>();

                view.layer = layer != UILayer.None ? layer : view.layer;

                view.SetParent(UIManager.Instance.GetParent(view.layer));

                view.SetName(panel.ToString());

                rect.Reset(); rect.SetFull();

                view.Init(panel); Show();

                status = Status.Display;
            }
            catch
            {
                status = Status.Error;
            }
        }

        protected virtual void Show()
        {
            if (view != null)
            {
                view.Refresh(paramter);

                view.SetActive(active);

                UIManager.Instance.SortDisplay(view.layer, view.transform);
            }
            callback?.Invoke(panel, active);
        }

        protected virtual void Hide()
        {
            if (view != null)
            {
                view.SetActive(active);
            }
            callback?.Invoke(panel, active);
        }

        public UIPanel panel { get; protected set; }

        public UILayer layer { get; protected set; }

        public bool active { get; protected set; }

        public int number { get; protected set; }

        enum Status
        {
            None,
            Loading,
            Display,
            Error,
        }
    }
}