using System;
using System.Windows.Input;

namespace KarateSemaphore
{
    public interface IModalDialogManager
    {
        void ShowDialog<T>(T viewModel, Func<T, ICommand> okAction);
    }
}