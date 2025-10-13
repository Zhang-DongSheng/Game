using Game.Data;

namespace Game.Logic
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
            _player.name = "Player";

            _player.sex = 1;

            _player.age = 18;

            _player.head = 1001;

            _player.frame = 2001;
        }
    }
}