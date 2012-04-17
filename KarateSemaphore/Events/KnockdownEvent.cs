using System;

namespace KarateSemaphore.Events
{
    public class KnockdownEvent : TimestampedEvent
    {
        private readonly ISemaphore _source;
        private readonly TimeSpan _initialTime;

        public KnockdownEvent(ISemaphore source)
        {
            _source = source;
            _initialTime = source.Time.Remaining;
        }

        public override string Display
        {
            get { return string.Concat(base.Display, " Knockdown ", _initialTime, " remaining"); }
        }

        public override void Redo()
        {
            _source.Time.Reset.Execute(TimeSpan.FromSeconds(10));
            _source.Time.StartStop.Execute(null);
        }

        public override void Undo()
        {
            _source.Time.Reset.Execute(_initialTime);
        }
    }
}
