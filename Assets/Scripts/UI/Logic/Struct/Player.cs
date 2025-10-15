namespace Game.Data
{
    public class Player
    {
        public string name;

        public int sex;

        public uint uid;

        public uint age;

        public uint head;

        public uint frame;

        public uint country;

        public string introduce;

        public void Copy(Player player)
        {
            this.name = player.name;

            this.sex = player.sex;

            this.uid = player.uid;

            this.age = player.age;

            this.head = player.head;

            this.frame = player.frame;

            this.country = player.country;

            this.introduce = player.introduce;
        }
    }
}