using System;
using UnityEngine;
using UnityEngine.Factory;

namespace Game.UI
{
    public class UICtrl
    {
        private UIBase view;

        private bool active;

        private UIStatus status = UIStatus.None;

        public void Open(UIKey key, UILayer layer)
        {
            active = true;

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
                view.Refresh(); view.Reopen();

                UIManager.Instance.SortDisplay(view.layer, view.transform);
            }
        }

        protected virtual void Hide()
        {
            if (view != null)
            {
                view.Close();
            }
        }

        private void Load(UIKey key, UILayer layer)
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

        private void LoadAsync(UIKey key, UILayer layer)
        {
            status = UIStatus.Loading;
        }

        private void Create(UIKey key, UILayer layer, GameObject go)
        {
            RectTransform rect = go.GetComponent<RectTransform>();

            view = go.GetComponent<UIBase>();

            view.layer = layer != UILayer.None ? layer : view.layer;

            view.SetParent(UIManager.Instance.GetParent(view.layer));

            view.SetName(key.ToString());

            rect.Reset(); rect.Full();

            view.Init();

            view.Refresh();

            view.SetActive(active);

            UIManager.Instance.SortDisplay(view.layer, go.transform);

            status = UIStatus.Done;
        }

        enum UIStatus
        {
            None,
            Loading,
            Done,
        }
    }
}