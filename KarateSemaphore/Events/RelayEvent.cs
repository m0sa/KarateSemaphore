using System;

namespace KarateSemaphore.Events
{
    public class RelayEvent : TimestampedEvent
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

        public override string Display
        {
            get { return string.Concat(base.Display, " ", _display); }
        }

        public override void Redo()
        {
            _redo();
        }

        public override void Undo()
        {
            _undo();
        }
    }
}
