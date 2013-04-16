#region

using KarateSemaphore.Core;
using KarateSemaphore.Core.Events;
using NSubstitute;
using NUnit.Framework;

#endregion

namespace KarateSemaphore.UnitTests
{
    [TestFixture]
    public class ChangeDisplayTextSettingsEventTest
    {
        private IDisplayTextSettings _settings;
        private ISemaphore _semaphore;
        private ChangeDisplayTextSettingsEvent _command;

        [SetUp]
        public void Setup()
        {
            _settings = Substitute.For<IDisplayTextSettings>();
            _settings.Aka.Returns("aka name");
            _settings.Ao.Returns("ao name");

            _semaphore = Substitute.For<ISemaphore>();
            _semaphore.Aka.DisplayText.Returns("aka original");
            _semaphore.Ao.DisplayText.Returns("ao original");

            _command = new ChangeDisplayTextSettingsEvent(_semaphore, _settings);
        }


        [Test]
        public void Redo_changes_display_names()
        {
            _command.Redo();

            Assert.That(_semaphore.Aka.DisplayText, Is.EqualTo("aka name"));
            Assert.That(_semaphore.Ao.DisplayText, Is.EqualTo("ao name"));
        }

        [Test]
        public void Undo_restores_changed_display_names()
        {
            _command.Redo();
            _command.Undo();

            Assert.That(_semaphore.Aka.DisplayText, Is.EqualTo("aka original"));
            Assert.That(_semaphore.Ao.DisplayText, Is.EqualTo("ao original"));
        }
        [Test]
        public void DisplayName_contains_both_new_names()
        {
            Assert.That(_command.Display, Is.StringContaining("aka name"));
            Assert.That(_command.Display, Is.StringContaining("ao name"));
        }
    }
}