using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UINotice : UIBase
    {
        [SerializeField] private Text txt_message;

        [SerializeField] private float duration;

        private float timer;

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > duration)
            {
                timer = 0; Close();
            }
        }

        public override void Refresh(params object[] paramter)
        {
            if (paramter == null) return;

            txt_message.text = paramter[0].ToString();
        }
    }
}