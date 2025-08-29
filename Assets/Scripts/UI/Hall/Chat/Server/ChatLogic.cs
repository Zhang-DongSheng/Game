using Game.Logic;

namespace Game.Logic
{
    public class ChatLogic : Singleton<ChatLogic>, ILogic
    {
        public void Initialize()
        {

        }

        public void RequestChat()
        {
            ScheduleLogic.Instance.Update(Schedule.Chat);
        }
    }
}