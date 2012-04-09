using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Threading;

namespace KarateSemaphore
{
    /// <summary> 
    /// The base class for MVVM style view models, implementing the <see cref="INotifyPropertyChanged"/> interface.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void OnPropertyChanged<T>(Expression<Func<T>> property)
        {
            OnPropertyChanged(ReflectionHelper.GetPropertyName(property));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var args = new PropertyChangedEventArgs(propertyName);
            Dispatcher.CurrentDispatcher.Invoke(new Action(() => PropertyChanged(this, args)));
        }
    }
}