namespace Game.State
{
    public interface IGameState
    {
        public void OnCreate();

        public void OnEnter();

        public void OnStay();

        public void OnExit();
    }
}