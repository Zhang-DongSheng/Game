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
            ing = true;
        }

        private void Update()
        {  
            if(ing)
            {
                step += Time.deltaTime * 1f;

                if (step < 1)
                {
                    OnChanged(step);
                }
                else
                {
                    OnCompleted();
                }

                ing = step < 1;
            }
        }

        private void OnChanged(float value)
        {
            progress.value = value;
        }

        private void OnCompleted()
        {
            UIManager.Instance.Open(UIKey.UIMain);
        }
    }
}