using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// Ã· æ√Ê∞Â
    /// </summary>
    public class UITips : UIBase
    {
        [SerializeField, Range(0, 10f)] private float interval = 3f;

        [SerializeField] private List<ItemTips> tips;

        private float timer;

        private int count;

        private readonly Stack<string> stack = new Stack<string>();

        protected override void OnUpdate(float delta)
        {
            if (timer + interval > Time.time) return;

            if (stack.Count == 0) return;

            count = tips.Count;

            for (int i = 0; i < count; i++)
            {
                if (tips[i].Active == false)
                {
                    tips[i].Startup(stack.Pop());

                    timer = Time.time;

                    break;
                }
            }
        }

        public override void Refresh(UIParameter paramter)
        {
            string messsage = paramter.Get<string>("tips");

            stack.Push(messsage);
        }
    }
}