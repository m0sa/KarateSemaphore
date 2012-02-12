using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Threading;

namespace WkfSemaphore
{
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