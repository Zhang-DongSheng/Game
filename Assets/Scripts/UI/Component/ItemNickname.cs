using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// 昵称
    /// </summary>
    public class ItemNickname
    {
        [SerializeField] private Text nick;

        public void Refresh(string name)
        {
            nick.text = name;
        }
    }
}
