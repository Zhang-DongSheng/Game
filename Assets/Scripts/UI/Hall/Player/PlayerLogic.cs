namespace Game.UI
{
    public class PlayerLogic : Singleton<PlayerLogic>, ILogic
    {
        private readonly PlayerInformation _player = new PlayerInformation();

        public PlayerInformation Player
        {
            get { return _player; }
        }

        public void Initialize()
        {

        }
    }
}