using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
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

        public void Release()
        {

        }
    }
}