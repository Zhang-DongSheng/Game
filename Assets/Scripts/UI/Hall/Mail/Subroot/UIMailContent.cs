using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIMailContent : ItemBase
    {
        [SerializeField] private Text text;

        public void Refresh(Mail mail)
        {
            text.text = mail.content;

            SetActive(true);
        }
    }
}
