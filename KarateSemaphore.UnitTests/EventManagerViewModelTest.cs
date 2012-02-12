using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using WkfSemaphore.Events;

namespace WkfSemaphore.UnitTests
{
    [TestFixture]
    public class EventManagerViewModelTest
    {
        private EventManagerViewModel _instance;

        [SetUp]
        public virtual void Setup()
        {
            _instance = new EventManagerViewModel();
        }

        private List<IEvent> AddAnExecuteMany(int count)
        {
            var actions = Enumerable.Range(0, count).Select(x => Substitute.For<IEvent>()).ToList();
            actions.ForEach(x => _instance.AddAndExecute(x));
            return actions;
        }

        [TestFixture]
        public class The_AddAndExecute_Method : EventManagerViewModelTest
        {
            [TestCase(ExpectedException = typeof(ArgumentNullException))]
            public void Throws_ArgumentNullException_When_Argument_Is_Null()
            {
                _instance.AddAndExecute(null);
            }

            [TestCase(true)]
            [TestCase(false)]
            public void Executes_Redo_On_Action_When_Invoked(bool empty)
            {
                if (!empty)
                {
                    _instance.AddAndExecute(Substitute.For<IEvent>());
                }

                var action = Substitute.For<IEvent>();

                _instance.AddAndExecute(action);

                action.Received().Redo();
            }

            [Test]
            public void Increases_Position()
            {
                _instance.AddAndExecute(Substitute.For<IEvent>());

                Assert.That(_instance.Position, Is.EqualTo(0));
            }

            [TestCase(3, 3)]
            [TestCase(3, 2)]
            [TestCase(3, 1)]
            [TestCase(3, 0)]
            public void Clears_The_Stack(int initialActions, int undoSteps)
            {
                AddAnExecuteMany(initialActions);

                _instance.Undo(undoSteps);
                _instance.AddAndExecute(Substitute.For<IEvent>());

                Assert.That(_instance.Events.Count, Is.EqualTo(initialActions - undoSteps + 1));
            }
        }

        [TestFixture]
        public class The_Undo_Method : EventManagerViewModelTest
        {
            [Test]
            public void Does_Nothing_When_On_Tail()
            {
                _instance.Undo(100);

                Assert.That(_instance.Position, Is.EqualTo(-1));
            }

            [TestCase(1, 1)]
            [TestCase(100, 1)]
            [TestCase(100, 30)]
            public void Undoes_The_Last_Redone_Action(int actionCount, int undoCount)
            {
                var actions = AddAnExecuteMany(actionCount);
                _instance.Undo(undoCount);

                actions.Last().Received(1).Undo();
            }
        }

        [TestFixture]
        public class The_Redo_Method : EventManagerViewModelTest
        {
            [Test]
            public void Does_Nothing_When_On_Tail()
            {
                _instance.Redo(100);

                Assert.That(_instance.Position, Is.EqualTo(-1));
            }

            [TestCase(1, 1)]
            [TestCase(1, 2)]
            [TestCase(100, 3)]
            public void Redoes_The_Last_Undone_Action(int actionCount, int undoRedoCount)
            {
                var actions = AddAnExecuteMany(actionCount);
                _instance.Undo(1);
                _instance.Redo(1);

                actions.Last().Received(2).Redo();
            }
        }

        [TestFixture]
        public class The_Clear_Method : EventManagerViewModelTest
        {
            [TestCase(1)]
            [TestCase(100)]
            public void Removes_All_Actions(int actionCount)
            {
                AddAnExecuteMany(actionCount);
                _instance.Clear();

                Assert.That(_instance.Events.Any(), Is.False);
            }

            [TestCase(1)]
            [TestCase(100)]
            public void Resets_The_Position(int actionCount)
            {
                AddAnExecuteMany(actionCount);
                _instance.Clear();

                Assert.That(_instance.Position, Is.EqualTo(-1));
            }

            [TestCase(1)]
            [TestCase(100)]
            public void Does_Not_Call_Undo(int actionCount)
            {
                var actions = AddAnExecuteMany(actionCount);
                _instance.Clear();

                foreach(var action in actions )
                {
                    action.DidNotReceiveWithAnyArgs().Undo();
                }
            }
        }
    }
}
