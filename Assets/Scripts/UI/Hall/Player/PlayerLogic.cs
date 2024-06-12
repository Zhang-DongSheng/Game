namespace Game.UI
{
    public class PlayerLogic : Singleton<PlayerLogic>, ILogic
    {
        private readonly Player _player = new Player();

        public Player Player
        {
            get { return _player; }
        }

        public void Initialize()
        {

        }
    }
}