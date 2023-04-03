using Data;
using Game.Resource;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public class CtrlBase
    {
        protected UIInformation information;

        protected UIParameter paramter;

        protected UIBase view;

        private Status status;

        private readonly Action<CtrlBase> callback;

        public CtrlBase(UIPanel panel, Action<CtrlBase> callback)
        {
            var config = DataManager.Instance.Load<DataUI>();

            this.information = config.Get(panel);

            this.callback = callback;
        }

        public void Open()
        {
            Open(true);
        }

        public void Open(bool async)
        {
            this.active = true;

            number++;

            switch (status)
            {
                case Status.None:
                    {
                        if (async)
                            LoadAsync();
                        else
                            Load();
                    }
                    break;
                case Status.Loading:
                    Debuger.LogWarning(Author.UI, string.Format("The panel of [{0}] is loading...", information.panel));
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
                    Debuger.LogError(Author.UI, string.Format("The panel of [{0}] resource was not found!", information.panel));
                    break;
            }
        }

        public void Close(bool destroy = false)
        {
            active = false; number--;

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
            callback?.Invoke(this);
        }

        public bool Record()
        {
            if (information == null) return false;

            return information.record;
        }

        public void Paramter(UIParameter paramter)
        {
            this.paramter = paramter;
        }

        private void Load()
        {
            status = Status.Loading;

            try
            {
                Object prefab = ResourceManager.Load(information.path);

                Create(prefab);
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.UI, e);
            }
        }

        private void LoadAsync()
        {
            status = Status.Loading;

            try
            {
                ResourceManager.LoadAsync(information.path, (asset) =>
                {
                    Create(asset);
                });
            }
            catch (Exception e)
            {
                Debuger.LogException(Author.UI, e);
            }
        }

        private void Create(Object prefab)
        {
            try
            {
                GameObject entity = GameObject.Instantiate(prefab) as GameObject;

                RectTransform rect = entity.GetComponent<RectTransform>();

                view = entity.GetComponent<UIBase>();

                view.layer = information.layer;

                view.order = information.order;

                view.SetParent(UIManager.Instance.GetParent(view.layer));

                view.SetName(information.name);

                rect.Reset(); rect.SetFull();

                view.Init(information.panel); Show();

                status = Status.Display;
            }
            catch
            {
                status = Status.Error;
            }
        }

        protected virtual void Show()
        {
            view.Refresh(paramter);

            view.Enter();

            callback?.Invoke(this);

            UIManager.Instance.Sort(view.layer, view.transform);
        }

        protected virtual void Hide()
        {
            view.Exit();
        }

        public bool active { get; protected set; }

        public int number { get; protected set; }

        public string name { get { return information.name; } }

        enum Status
        {
            None,
            Loading,
            Display,
            Error,
        }
    }
}