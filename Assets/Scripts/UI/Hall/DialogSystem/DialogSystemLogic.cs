using Game.Data;
using Game.UI;
using System;
using System.Collections.Generic;

namespace Game
{
    public class DialogSystemLogic : Singleton<DialogSystemLogic>, ILogic
    {
        private uint current;

        private bool complete;

        private DataDialog _dialog;

        private List<DialogRoleInformation> _roles = new List<DialogRoleInformation>();

        public List<DialogRoleInformation> Roles
        { 
            get { return _roles; }
        }

        public void Initialize()
        {
            current = 1;
        }

        public void Refresh(Action callback)
        {
            DataManager.Instance.LoadAsync<DataDialog>((data) =>
            {
                _dialog = data;

                current = _dialog.start;

                complete = current == _dialog.end;

                callback?.Invoke();
            });
        }

        public void Ready()
        {
            
        }

        public DialogInformation Next()
        {
            if (current == 0 || complete) return null;

            var info = _dialog.Get(current);

            current = info.next;

            complete = current == _dialog.end;

            return info;
        }

        public DialogInformation Option(uint value)
        {
            current = value;

            var info = _dialog.Get(current);

            current = info.next;

            complete = current == _dialog.end;

            return info;
        }
    }
}