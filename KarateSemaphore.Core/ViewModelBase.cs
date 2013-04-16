#region

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;

#endregion

namespace KarateSemaphore.Core
{
    /// <summary>
    ///   The base class for MVVM style view models, implementing the <see cref="System.ComponentModel.INotifyPropertyChanged" /> interface.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void OnPropertyChanged<T>(Expression<Func<T>> property)
        {
            OnPropertyChanged(ReflectionHelper.GetPropertyName(property));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            SynchronizationContext.Current.Post(args => 
                PropertyChanged(this, (PropertyChangedEventArgs)args), 
                new PropertyChangedEventArgs(propertyName));
        }

        #region IDisposable

        ~ViewModelBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion
    }
}