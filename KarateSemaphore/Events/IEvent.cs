namespace KarateSemaphore.Events
{
    public interface IEvent
    {
        string Display { get; }
        void Redo();
        void Undo();
    }
}