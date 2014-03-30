using System;
using System.Collections.Generic;
using System.Threading;
using KarateSemaphore.Core;
using NUnit.Framework;

namespace KarateSemaphore.UnitTests
{

    public sealed class TestSynchronizationContext : SynchronizationContext
    {
        public override void Send(SendOrPostCallback d, object state)
        {
            d(state);
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            d(state);
        }
    }
    [SetUpFixture]
    public class Setup
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            var ctx = new TestSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(ctx);
        }
    }


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
