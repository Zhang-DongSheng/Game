using Game.Data;
using Game.UI;

namespace Game.Logic
{
    public class PlayerLogic : Singleton<PlayerLogic>, ILogic
    {
        private readonly Player _player = new Player();

        private readonly Player _cache = new Player();

        public Player Player
        {
            get { return _player; }
        }

        public Player Cache
        {
            get { return _cache; }
        }

        public void Initialize()
        {
            _player.name = "Player";

            _player.sex = 1;

            _player.age = 18;

            _player.head = 1001;

            _player.frame = 2001;
        }

        public void SetHead(uint head, out Status status)
        {
            _cache.head = head;

            status = Status.Undone;

            if (_player.head == _cache.head)
            {
                status = Status.Claimed;
            }
            else if (true)
            {
                status = Status.Available;
            }
        }

        public void SetFrame(uint frame, out Status status)
        {
            _cache.frame = frame;

            status = Status.Undone;

            if (_player.frame == _cache.frame)
            {
                status = Status.Claimed;
            }
            else if (true)
            {
                status = Status.Available;
            }
        }

        public void SetNickname(string name, out Status status)
        {
            _cache.name = name;

            if (_player.name == _cache.name)
            {
                status = Status.Claimed;
            }
            else
            {
                status = Status.Available;
            }
        }

        public void SetCountry(uint country, out Status status)
        {
            _cache.country = country;

            if (_player.country == _cache.country)
            {
                status = Status.Claimed;
            }
            else
            {
                status = Status.Available;
            }
        }
    }
}