using Game.Data;
using Game.UI;

namespace Game
{
    public class DialogSystemLogic : Singleton<DialogSystemLogic>, ILogic
    {
        private DataDialog dialog;

        private uint current;

        private bool display;

        private bool complete;

        public void Initialize()
        {
            current = 1;
        }

        public void Refresh()
        {
            DataManager.Instance.LoadAsync<DataDialog>((data) =>
            {
                dialog = data;

                current = dialog.start;

                complete = current == dialog.end;
            });
        }

        public DialogInformation Next()
        {
            if (current == 0 || complete) return null;

            var info = dialog.Get(current);

            current = info.next;

            complete = current == dialog.end;

            return info;
        }

        public bool Display { get; set; }
    }
}
