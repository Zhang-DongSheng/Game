using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class FriendView : ViewBase
    {
        public override void Refresh(UIParameter parameter)
        {
            var list = FriendLogic.Instance.Friends;
        }
    }
}