#region

using KarateSemaphore.Events;
using NSubstitute;
using NUnit.Framework;

#endregion

namespace KarateSemaphore.UnitTests
{
    [TestFixture]
    public class ChangeDisplayNameSettingsEventTest
    {
        private IDisplayNameSettings _settings;
        private ISemaphore _semaphore;
        private ChangeDisplayNameSettingsEvent _command;

        [SetUp]
        public void Setup()
        {
            _settings = Substitute.For<IDisplayNameSettings>();
            _settings.Aka.Returns("aka name");
            _settings.Ao.Returns("ao name");

            _semaphore = Substitute.For<ISemaphore>();
            _semaphore.Aka.DisplayName.Returns("aka original");
            _semaphore.Ao.DisplayName.Returns("ao original");

            _command = new ChangeDisplayNameSettingsEvent(_semaphore, _settings);
        }


        [Test]
        public void Redo_changes_display_names()
        {
            _command.Redo();

            Assert.That(_semaphore.Aka.DisplayName, Is.EqualTo("aka name"));
            Assert.That(_semaphore.Ao.DisplayName, Is.EqualTo("ao name"));
        }

        [Test]
        public void Undo_restores_changed_display_names()
        {
            _command.Redo();
            _command.Undo();

            Assert.That(_semaphore.Aka.DisplayName, Is.EqualTo("aka original"));
            Assert.That(_semaphore.Ao.DisplayName, Is.EqualTo("ao original"));
        }
        [Test]
        public void DisplayName_contains_both_new_names()
        {
            Assert.That(_command.Display, Is.StringContaining("aka name"));
            Assert.That(_command.Display, Is.StringContaining("ao name"));
        }
    }
}