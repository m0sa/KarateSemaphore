using System.Collections.Generic;

namespace KarateSemaphore.Events
{
    public interface IEventManager
    {
        ICollection<IEvent> Events { get; }
        int Position { get; }
        void AddAndExecute(IEvent @event);
        void Redo(int steps);
        void Undo(int steps);
        void Clear();
    }
}