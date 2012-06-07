using KarateSemaphore.Events;
using NSubstitute;
using NUnit.Framework;

namespace KarateSemaphore.UnitTests
{
    [TestFixture]
    public class DisplayNameEditorViewModelTest
    {
        [Test]
        public void ChangeDisplayNamesCommand_executes_ChangeDisplayName_event()
        {
            var semaphore = Substitute.For<ISemaphore>();
            
            var instance = new DisplayNameEditorViewModel(semaphore);
            instance.ChangeDisplayNames.Execute(null);

            semaphore.EventManager.Received().AddAndExecute(Arg.Any<ChangeDisplayNameSettingsEvent>());
        }
    }
}