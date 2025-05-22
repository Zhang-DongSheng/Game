namespace Game.UI
{
    public abstract class ConsoleBase : ItemBase
    {
        public abstract string Name { get; }

        public virtual void Initialize()
        {
            
        }

        public virtual void Refresh(float delta)
        {
            
        }

        public virtual void Dispose()
        {

        }
    }
}