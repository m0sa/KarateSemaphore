#region

using System;
using System.Linq;
using System.Windows.Input;
using KarateSemaphore.Events;
using NSubstitute;
using NUnit.Framework;

#endregion

namespace KarateSemaphore.UnitTests
{
    public class SemaphoreTest
    {
        public class Given_A_Semaphore : ISemaphore
        {
            private readonly ISemaphore _sut;

            public Given_A_Semaphore(IStopWatch stopWatch = null, IEventManager eventManager = null, IModalDialogManager modalDialogManager = null, ICompetitor aka = null, ICompetitor ao = null)
            {
                var _aka = Substitute.For<ICompetitor>();
                _aka.Belt.Returns(x => Belt.Aka);
                var _ao = Substitute.For<ICompetitor>();
                _ao.Belt.Returns(x => Belt.Ao);
                var _stopwatch = Substitute.For<IStopWatch>();
                var _eventManager = Substitute.For<IEventManager>();
                var _modalDialogManager = Substitute.For<IModalDialogManager>();
                _sut = new SemaphoreViewModel(stopWatch ?? _stopwatch, eventManager ?? _eventManager, modalDialogManager ?? _modalDialogManager, aka ?? _aka, ao ?? _ao);
            }
            
            protected ISemaphore Sut
            {
                get { return _sut; }
            }

            public TimeSpan ResetTime
            {
                get { return Sut.ResetTime; }
                set { Sut.ResetTime = value; }
            }

            public IEventManager EventManager
            {
                get { return Sut.EventManager; }
            }

            public ICommand Reset
            {
                get { return Sut.Reset; }
            }

            public IStopWatch Time
            {
                get { return Sut.Time; }
            }

            public ICompetitor Aka
            {
                get { return Sut.Aka; }
            }

            public ICompetitor Ao
            {
                get { return Sut.Ao; }
            }

            public bool IsKnockdown
            {
                get { return Sut.IsKnockdown; }
            }

            public ICommand ToggleKnockdownMode
            {
                get { return Sut.ToggleKnockdownMode; }
            }

            public ICommand RequestDisplayNameChange
            {
                get { return Sut.RequestDisplayNameChange; }
            }
        }

        [TestFixture]
        public abstract class When_Reset_command_executed
        {
            private ISemaphore _sut;
            protected abstract ISemaphore CreateSut();

            [SetUp]
            public virtual void Setup()
            {
                _sut = CreateSut();
                _sut.ResetTime = TimeSpan.FromSeconds(30);

                _sut.Reset.Execute(null);
            }

            [Test]
            public void Stopwatch_is_reset()
            {
                _sut.Time.Reset.Received().Execute(_sut.ResetTime);
            }

            [Test]
            public void Aka_is_reset()
            {
                _sut.Aka.ChangeC1.Received().Execute(Penalty.None);
                _sut.Aka.ChangeC2.Received().Execute(Penalty.None);
                _sut.Aka.ChangePoints.Received().Execute(Award.None);
            }

            [Test]
            public void Ao_is_reset()
            {
                _sut.Ao.ChangeC1.Received().Execute(Penalty.None);
                _sut.Ao.ChangeC2.Received().Execute(Penalty.None);
                _sut.Ao.ChangePoints.Received().Execute(Award.None);
            }

            [Test]
            public void EventManager_is_reset()
            {
                _sut.EventManager.Received().Clear();
            }
        }

        public class And_match_is_in_the_middle : When_Reset_command_executed
        {
            protected override ISemaphore CreateSut()
            {
                var instance = new Given_A_Semaphore();
                instance.Time.Remaining.Returns(TimeSpan.FromSeconds(25));
                return instance;
            }
        }

        public class And_match_is_completed : When_Reset_command_executed
        {
            protected override ISemaphore CreateSut()
            {
                var instance = new Given_A_Semaphore();
                instance.Time.Remaining.Returns(TimeSpan.Zero);
                return instance;
            }
        }

        public class When_ToggleKnockdownMode_command_executed_once
        {
            private ISemaphore _sut;

            [SetUp]
            public void Setup()
            {
                _sut = new Given_A_Semaphore(eventManager: new EventManagerViewModel() );

                _sut.ToggleKnockdownMode.Execute(null);
            }

            [Test]
            public void IsKnockdown_is_true()
            {
                Assert.That(_sut.IsKnockdown, Is.True);
            }

            [Test]
            public void Stopwatch_was_reset_to_10_seconds_and_started()
            {
                _sut.Time.Reset.Received().Execute(TimeSpan.FromSeconds(10));
                _sut.Time.StartStop.Received().Execute(null);
            }
        }

        public class When_ToggleKnockdownMode_command_executed_twice_in_the_middle_of_the_match
        {
            private ISemaphore _sut;
            private TimeSpan _matchTime;

            [SetUp]
            public void Setup()
            {
                _matchTime = TimeSpan.FromSeconds(31);
                _sut = new Given_A_Semaphore(eventManager: new EventManagerViewModel());
                _sut.Time.Remaining.Returns(_matchTime);
                _sut.ToggleKnockdownMode.Execute(null);

                _sut.Time.Reset.ClearReceivedCalls();
                _sut.Time.StartStop.ClearReceivedCalls();
                _sut.ToggleKnockdownMode.Execute(null);
            }

            [Test]
            public void IsKnockdown_is_false()
            {
                Assert.That(_sut.IsKnockdown, Is.False);
            }

            [Test]
            public void Stopwatch_was_reset_to_match_time()
            {
                _sut.Time.Reset.Received().Execute(_matchTime);
            }

            [Test]
            public void Stopwatch_was_not_started()
            {
                _sut.Time.StartStop.DidNotReceiveWithAnyArgs().Execute(null);
            }
        }
    
        [TestFixture]
        public class InteractionTest
        {
            [Test]
            public void Modal_dialog_is_requested_on_RequestDisplayNameChange_command()
            {
                var modalDialogManager = Substitute.For<IModalDialogManager>();
                var instance = new Given_A_Semaphore(modalDialogManager: modalDialogManager);
                instance.RequestDisplayNameChange.Execute(null);

                modalDialogManager.Received().ShowDialog(
                    Arg.Any<DisplayNameEditorViewModel>(), Arg.Any<Func<DisplayNameEditorViewModel, ICommand>>());
            }
        }
    }
}