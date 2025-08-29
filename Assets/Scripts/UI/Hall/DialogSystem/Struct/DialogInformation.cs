namespace Game.Data
{
    public class DialogRoleInformation
    {
        public string name;

        public string sprite;

        public uint position;

        public DialogRoleInformation(DialogInformation dialog)
        {
            name = dialog.role;

            var split = dialog.content.Split(',');

            sprite = split[0];

            position = System.Convert.ToUInt32(split[1]);
        }
    }

    public class DialogBackgroundInformation
    {
        public string sprite;
    }
}