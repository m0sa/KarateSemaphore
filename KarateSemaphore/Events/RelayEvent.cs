using System;

namespace KarateSemaphore.Events
{
    public class RelayEvent : IEvent
    {
        private readonly Action _redo;
        private readonly Action _undo;
        private readonly string _display;

        public RelayEvent(string display = null, Action redo = null, Action undo = null)
        {
            _display = display ?? string.Empty;
            _redo = redo ?? delegate { };
            _undo = undo ?? delegate { };
        }

        public string Display
        {
            get { return _display; }
        }

        public void Redo()
        {
            _redo();
        }

        public void Undo()
        {
            _undo();
        }
    }
}
