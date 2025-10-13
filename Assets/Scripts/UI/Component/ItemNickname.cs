using UnityEngine;

namespace Game.UI
{
    /// <summary>
    /// 昵称
    /// </summary>
    public class ItemNickname : ItemBase
    {
        [SerializeField] private TextBind txtNick;

        public void Refresh(string name)
        {
            txtNick.SetTextImmediately(name);
        }
    }
}