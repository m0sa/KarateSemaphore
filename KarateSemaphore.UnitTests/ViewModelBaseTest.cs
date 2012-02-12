using System;

using NUnit.Framework;

namespace KarateSemaphore.UnitTests
{
    [TestFixture]
    public class ViewModelBaseTest
    {
        private class TestableViewModelBase : ViewModelBase
        {
            private string _property;

            public string Property
            {
                get { return _property; }
                set 
                { 
                    _property = value; 
                    OnPropertyChanged(() => Property); 
                }
            }
        }

        [Test]
        public void INotifyPropertyChanged_Works_As_Expected()
        {
            var eventRaised = false;
            var instance = new TestableViewModelBase();
            instance.PropertyChanged += (s, e) => eventRaised = true;

            instance.Property = Guid.NewGuid().ToString();

            Assert.That(eventRaised, Is.True);
        }
    }
}
