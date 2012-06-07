using KarateSemaphore.Events;
using NSubstitute;
using NUnit.Framework;

namespace KarateSemaphore.UnitTests
{
    [TestFixture]
    public class DisplayTextEditorViewModelTest
    {
        [Test]
        public void ChangeDisplayNamesCommand_executes_ChangeDisplayName_event()
        {
            var semaphore = Substitute.For<ISemaphore>();
            
            var instance = new DisplayTextEditorViewModel(semaphore);
            instance.ChangeDisplayText.Execute(null);

            semaphore.EventManager.Received().AddAndExecute(Arg.Any<ChangeDisplayTextSettingsEvent>());
        }
    }
}