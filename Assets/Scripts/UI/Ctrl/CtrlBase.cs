using Data;
using Game.Resource;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public class CtrlBase
    {
        protected UIParameter paramter;

        protected UIBase view;

        private Status status;

        public CtrlBase(UIPanel panel)
        {
            var config = DataManager.Instance.Load<DataUI>();

            information = config.Get(panel);

            if (information == null)
            {
                Debuger.LogError(Author.UI, "Config is Null!");
            }
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

        public void Paramter(UIParameter paramter)
        {
            this.paramter = paramter;
        }

        public void Close(bool destroy = false)
        {
            active = false; number--;

            if (view == null) return;

            if (destroy || information.destroy)
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
            UIManager.Instance.Record(this);
        }

        public void Display(bool active)
        {
            if (this.active == active) return;

            this.active = active;

            if (view == null) return;

            if (active)
            {
                Show(); number++;
            }
            else
            {
                Hide(); number--;
            }
        }

        public virtual bool Back()
        {
            if (view != null)
            {
                return view.Back();
            }
            return false;
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
            catch (Exception e)
            {
                Debuger.LogError(Author.UI, e.Message);

                status = Status.Error;
            }
        }

        protected virtual void Show()
        {
            view.Refresh(paramter);

            view.Enter();

            UIManager.Instance.Sort(view.layer, view.transform);

            UIManager.Instance.Record(this);
        }

        protected virtual void Hide()
        {
            view.Exit();
        }

        public bool active { get; private set; }

        public int number { get; private set; }

        public UIInformation information { get; private set; }

        enum Status
        {
            None,
            Loading,
            Display,
            Error,
        }
    }
}