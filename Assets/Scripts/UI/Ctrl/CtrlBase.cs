using Game.Resource;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public class CtrlBase
    {
        protected Paramter paramter;

        protected UIBase view;

        private Status status;

        private bool async;

        private readonly Action<UIPanel, UIType, bool> callback;

        public CtrlBase(UIPanel panel, Action<UIPanel, UIType, bool> callback)
        {
            this.panel = panel;

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

        public void Close(bool destroy = false)
        {
            active = false;

            if (view == null) return;

            if (destroy)
            {
                if (view.gameObject != null)
                {
                    GameObject.Destroy(view.gameObject);
                }
                status = Status.None;
            }
            else
            {
                Hide();
            }
            callback?.Invoke(panel, view.type, active);
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
            if (view == null) return;

            view.Refresh(paramter);

            view.SetActive(active);

            callback?.Invoke(panel, view.type, active);

            UIManager.Instance.Sort(view.layer, view.transform);
        }

        protected virtual void Hide()
        {
            if (view != null)
            {
                view.SetActive(active);
            }
            number--;
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