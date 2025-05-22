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

        private readonly List<uint> _history = new List<uint>();

        private readonly List<DialogRoleInformation> _roles = new List<DialogRoleInformation>();

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
            UIQuickEntry.Open(UIPanel.Waiting);

            DataManager.Instance.LoadAsync<DataDialog>((data) =>
            {
                _dialog = data;

                current = _dialog.start;

                complete = current == _dialog.end;

                _roles.Clear();

                for (int i = 0; i < _history.Count; i++)
                {
                    var info = _dialog.Get(current);

                    if (info == null) continue;

                    Execute(info);
                }
                callback?.Invoke();

                UIQuickEntry.Close(UIPanel.Waiting);
            });
        }

        public DialogInformation Next()
        {
            if (current == 0 || complete) return null;

            var info = _dialog.Get(current);

            _history.Add(current);

            Execute(info);

            current = info.next;

            complete = current == _dialog.end;

            if (info.type == DialogType.Content ||
                info.type == DialogType.Option)
            {
                return info;
            }
            else
            {
                return Next();
            }
        }

        public DialogInformation Option(uint option)
        {
            current = option;

            var info = _dialog.Get(current);

            _history.Add(current);

            Execute(info);

            current = info.next;

            complete = current == _dialog.end;

            if (info.type == DialogType.Content ||
                info.type == DialogType.Option)
            {
                return info;
            }
            else
            {
                return Next();
            }
        }

        private void Execute(DialogInformation dialog)
        {
            switch (dialog.type)
            {
                case DialogType.Player:
                    {
                        var index = _roles.FindIndex(x => x.name == dialog.role);

                        if (dialog.parameter == "true")
                        {
                            if (index > -1)
                            {
                                _roles[index] = new DialogRoleInformation(dialog);
                            }
                            else
                            {
                                _roles.Add(new DialogRoleInformation(dialog));
                            }
                        }
                        else if (index > -1)
                        {
                            _roles.RemoveAt(index);
                        }
                    }
                    break;
                case DialogType.Background:
                    {

                    }
                    break;
            }
        }
    }
}