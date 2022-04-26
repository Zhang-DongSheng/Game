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

        private Status status = Status.None;

        public void Open(UIPanel panel, UILayer layer)
        {
            this.panel = panel;

            this.layer = layer;

            Open();
        }

        public void Open()
        {
            active = true; number++;

            switch (status)
            {
                case Status.None:
                    if (GameConfig.Load == LoadType.AssetBundle)
                    {
                        LoadAsync(panel, layer);
                    }
                    else
                    {
                        Load(panel, layer);
                    }
                    break;
                case Status.Loading:
                    Debuger.LogWarning(Author.UI, string.Format("The panel of [{0}] is loading...", panel));
                    break;
                case Status.Display:
                    if (view != null)
                    {
                        view.Reopen(); Show();
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
                status = Status.None;
            }
            else
            {
                Hide();
            }
        }

        private void Load(UIPanel key, UILayer layer)
        {
            status = Status.Loading;

            try
            {
                Object prefab = ResourceManager.Load(string.Format("Package/Prefab/UI/Panel/{0}.prefab", key));

                Create(key, layer, prefab);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void LoadAsync(UIPanel key, UILayer layer)
        {
            status = Status.Loading;

            try
            {
                ResourceManager.LoadAsync(string.Format("Package/Prefab/UI/Panel/{0}.prefab", key), (asset) =>
                {
                    Create(key, layer, asset);
                });
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void Create(UIPanel key, UILayer layer, Object prefab)
        {
            try
            {
                GameObject entity = GameObject.Instantiate(prefab) as GameObject;

                RectTransform rect = entity.GetComponent<RectTransform>();

                view = entity.GetComponent<UIBase>();

                view.layer = layer != UILayer.None ? layer : view.layer;

                view.SetParent(UIManager.Instance.GetParent(view.layer));

                view.SetName(key.ToString());

                rect.Reset(); rect.SetFull();

                view.Init(); Show();

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
            EventManager.Post(EventKey.UIOpen);
        }

        protected virtual void Hide()
        {
            if (view != null)
            {
                view.SetActive(false);
            }
            EventManager.Post(EventKey.UIClose);
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