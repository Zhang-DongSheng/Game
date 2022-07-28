using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// Ã· æ√Ê∞Â
    /// </summary>
    public class UITips : UIBase
    {
        [SerializeField] private ItemTips tips;

        private Status status;

        private readonly Stack<string> stack = new Stack<string>();

        private void Awake()
        {
            tips.callback = Next;
        }

        private void Compute()
        {
            if (status == Status.None || status == Status.Next)
            {
                status = stack.Count > 0 ? Status.Ready : Status.Complete;

                switch (status)
                {
                    case Status.Ready:
                        {
                            tips.Startup(stack.Pop());

                            status = Status.Display;
                        }
                        break;
                    case Status.Complete:
                        {
                            OnClickClose();

                            status = Status.None;
                        }
                        break;
                }
            }
        }

        private void Next()
        {
            status = Status.Next;

            Compute();
        }

        public override void Refresh(Paramter paramter)
        {
            string messsage = paramter.Get<string>("tips");

            stack.Push(messsage);

            Compute();
        }
        enum Status
        {
            None,
            Ready,
            Display,
            Next,
            Complete,
        }
    }
}