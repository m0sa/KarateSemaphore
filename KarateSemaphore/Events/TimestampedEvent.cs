using System;

namespace KarateSemaphore.Events
{
    public abstract class TimestampedEvent : IEvent
    {
        private readonly DateTime _timestamp;

        protected  TimestampedEvent()
        {
            _timestamp = DateTime.Now;
        }

        public virtual string Display
        {
            get { return _timestamp.ToString("HH:mm:ss"); }
        }

        public abstract void Redo();
        public abstract void Undo();
    }
}