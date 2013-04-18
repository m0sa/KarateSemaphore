#region

using System;
using System.Linq;
using System.Reactive;
using KarateSemaphore.Core;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

#endregion

namespace KarateSemaphore.UnitTests
{
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
        public virtual void Setup()
        {
            _scheduler = new TestScheduler();
            _interval = TimeSpan.FromMinutes(1);
            var now = DateTime.Now;
            var timeSchedule = _scheduler.CreateHotObservable(
                Enumerable
                    .Range(0, 181)
                    .Select(i => new Recorded<Notification<DateTime>>(i, Notification.CreateOnNext(now.AddSeconds(i))))
                    .ToArray());
            _instance = new StopWatchViewModel(timeSchedule);
            _instance.Reset.Execute(_interval);
        }

        [TearDown]
        public void Teardown()
        {
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

        protected void Delta(int seconds)
        {
            var parameter = TimeSpan.FromSeconds(seconds);
            if (_instance.Delta.CanExecute(parameter))
            {
                _instance.Delta.Execute(parameter);
            }
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
        public void When_paused_time_is_not_updated()
        {
            StartStop();
            _scheduler.AdvanceBy(1); // 1 sec
            StartStop(); // Pause
            _scheduler.AdvanceBy(2); // 2 sec, paused...
            StartStop(); // Continue
            _scheduler.AdvanceBy(1); // 1 sec

            Assert.That(_instance.Remaining, Is.EqualTo(TimeSpan.FromSeconds(58)));
        }

        [Test]
        public void When_10_seconds_remaining_the_Atoshibaraku_event_is_raised()
        {
            var eventRaised = false;
            _instance.Atoshibaraku += (s, e) => eventRaised = true;
            StartStop();
            _scheduler.AdvanceBy(50);

            Assert.That(_instance.Remaining, Is.EqualTo(TimeSpan.FromSeconds(10)));
            Assert.IsTrue(eventRaised);
        }

        [Test]
        public void When_less_than_10_seconds_remaining_Atoshibaraku_event_is_raised_only_once()
        {
            var eventCount = 0;
            _instance.Atoshibaraku += (s, e) => eventCount++;
            StartStop();
            _scheduler.AdvanceBy(55);

            Assert.That(eventCount, Is.EqualTo(1));
        }

        [Test]
        public void After_interval_elapses_time_is_always_zero()
        {
            StartStop();
            _scheduler.AdvanceBy(65);

            Assert.That(_instance.Remaining, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void After_interval_elapses_time_the_MatchEnd_event_is_raised()
        {
            var eventRaised = false;
            _instance.MatchEnd += (s, e) => eventRaised = true;
            StartStop();
            _scheduler.AdvanceBy(60);

            Assert.IsTrue(eventRaised);
        }

        [Test]
        public void After_interval_elapses_time_the_MatchEnd_event_is_raised_only_once()
        {
            var eventCount = 0;
            _instance.MatchEnd += (s, e) => eventCount++;
            StartStop();
            _scheduler.AdvanceBy(65);

            Assert.That(eventCount, Is.EqualTo(1));
        }

        [Test]
        public void After_completed_and_minus_delta_can_be_completed_again()
        {
            var eventCount = 0;
            _instance.MatchEnd += (s, e) => eventCount++;
            StartStop();
            _scheduler.AdvanceBy(60);
            StartStop();
            Delta(-1);
            StartStop();
            _scheduler.AdvanceBy(1);

            Assert.That(eventCount, Is.EqualTo(2));
        }

        [Test]
        public void Cannot_execute_Delta_when_running()
        {
            StartStop(); // running

            var result = _instance.Delta.CanExecute(TimeSpan.FromSeconds(1));

            Assert.IsFalse(result);
        }

        public class And_one_match_was_completed : Given_a_StopWatchViewModel_and_an_one_minute_interval
        {
            public override void Setup()
            {
                base.Setup();
                StartStop();
                Scheduler.AdvanceBy(60);
                Reset();
            }
        }
    }
}