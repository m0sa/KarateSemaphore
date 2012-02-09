using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace WkfSemaphore
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void OnPropertyChanged(string propertyName)
        {
            var args = new PropertyChangedEventArgs(propertyName);
            Dispatcher.CurrentDispatcher.Invoke(new Action(() => PropertyChanged(this, args)));
        }
    }
}