using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UILoading : UIBase
    {
        [SerializeField] private Slider progress;

        private float step;

        private bool ing;

        private void Awake()
        {
            Ready();
        }

        private void Update()
        {  
            if(ing)
            {
                step += Time.deltaTime * 1f;

                if (step < 1)
                {
                    Run(step);
                }
                else
                {
                    Completed();
                }

                ing = step < 1;
            }
        }

        public void Ready()
        {
            step = 0; ing = true;
        }

        private void Run(float value)
        {
            progress.value = value;
        }

        private void Completed()
        {
            UIManager.Instance.Open(UIKey.UIMain);
        }
    }
}