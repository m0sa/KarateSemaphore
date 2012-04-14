using System;
using System.Linq;

using NUnit.Framework;

namespace KarateSemaphore.UnitTests
{
    using System.Reactive;
    using Microsoft.Reactive.Testing;

    [TestFixture]
    public class Given_a_StopWatchViewModel_and_an_one_minute_interval
    {
        private TestScheduler _scheduler;
        private StopWatchViewModel _instance;
        private TimeSpan _interval;

        public TestScheduler Scheduler
        {
            get { return _scheduler; }
        }

        [SetUp]
        public void Setup()
        {
            _scheduler = new TestScheduler();
            _interval = TimeSpan.FromMinutes(1);
            var now = DateTime.Now;
            var timeSchedule = _scheduler.CreateHotObservable(
                Enumerable
                .Range(0, 61)
                .Select(i => new Recorded<Notification<DateTime>>(i, Notification.CreateOnNext(now.AddSeconds(i))))
                .ToArray());
            _instance = new StopWatchViewModel(timeSchedule);
            _instance.Reset.Execute(_interval);
        }

        [TearDown]
        public void Teardown() {
            _instance.Dispose();
        }

        protected void Reset()
        {
            _instance.Reset.Execute(_interval);
        }

        protected void StartStop()
        {
            _instance.StartStop.Execute(null);
        }

        [Test]
        public void When_reset_dont_progress()
        {
            StartStop();
            _scheduler.AdvanceBy(1);
            Reset();
            _scheduler.AdvanceBy(1);

            Assert.That(_instance.Remaining, Is.EqualTo(_interval));
        }

        [Test]
        public void The_remaining_time_is_updated_properly()
        {
            Reset();
            StartStop();
            _scheduler.AdvanceBy(3);

            Assert.That(_instance.Remaining, Is.EqualTo(TimeSpan.FromSeconds(57)));
        }

        [Test]
        public void Time_is_not_updated_while_paused()
        {
            StartStop();
            _scheduler.AdvanceBy(1);          // 1 sec
            StartStop();                      // Pause
            _scheduler.AdvanceBy(2);          // 2 sec, paused...
            StartStop();                      // Continue
            _scheduler.AdvanceBy(1);          // 1 sec
            
            Assert.That(_instance.Remaining, Is.EqualTo(TimeSpan.FromSeconds(58)));
        }
    }
}
