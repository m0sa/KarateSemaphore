namespace KarateSemaphore.Events
{
    public interface IEventManager
    {
        void AddAndExecute(IEvent @event);
        void Redo(int steps);
        void Undo(int steps);
        void Clear();
    }
}