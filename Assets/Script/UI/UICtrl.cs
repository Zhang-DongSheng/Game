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
                    Create(key, layer);
                    break;
                case UIStatus.Done:
                    SetActive(active);
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
                SetActive(active);
            }
        }

        public void Create(UIKey key, UILayer layer)
        {
            status = UIStatus.Loading;

            try
            {
                GameObject go = Factory.Instance.Pop(key.ToString()) as GameObject;

                view = go.GetComponent<UIBase>();

                view.layer = layer != UILayer.None ? layer : view.layer;

                view.SetParent(UIManager.Instance.GetParent(view.layer));

                view.SetName(key.ToString());

                view.Reset();

                view.SetActive(active);

                status = UIStatus.Done;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void SetActive(bool active)
        {
            if (view != null)
            {
                view.SetActive(active);
            }
        }

        enum UIStatus
        {
            None,
            Loading,
            Done,
        }
    }
}