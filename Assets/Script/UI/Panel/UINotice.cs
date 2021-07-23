using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UINotice : UIBase
    {
        [SerializeField] private RectTransform content;

        [SerializeField] private Text notice;

        [SerializeField, Range(0, 10)] private float duration;

        private float timer;

        private Status status;

        private void Update()
        {
            switch (status)
            {
                case Status.Update:
                    {
                        timer += Time.deltaTime;

                        if (timer > duration)
                        {
                            status = Status.Complete;
                        }
                        else
                        {
                            OnValueChanged();
                        }
                    }
                    break;
                case Status.Complete:
                    {
                        OnCompleted();
                    }
                    break;
            }
        }

        public override void Refresh(Paramter paramter)
        {
            status = Status.Idle;

            notice.text = paramter.Get<string>("notice");

            timer = 0;

            status = Status.Update;
        }

        private void OnValueChanged()
        {

        }

        private void OnCompleted()
        {
            status = Status.Idle;

            UIManager.Instance.Close(UIPanel.UINotice);
        }

        enum Status
        {
            Idle,
            Update,
            Complete,
        }
    }
}