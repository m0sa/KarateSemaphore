using System;
using System.Diagnostics;
using System.Windows.Input;

namespace KarateSemaphore.Core
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T) parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManagerProvider.Instance.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManagerProvider.Instance.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            T value = default(T);

            if (parameter != null && typeof (T).IsInstanceOfType(parameter))
            {
                value = (T) parameter;
            }
            else if (typeof (T).IsEnum)
            {
                value = (T) Enum.Parse(typeof (T), parameter + string.Empty, true);
            }
            _execute(value);
        }

        #endregion
    }

    public class RelayCommand : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManagerProvider.Instance.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManagerProvider.Instance.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        #endregion
    }
}