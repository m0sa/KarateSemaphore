using System;
using System.Windows.Input;
using KarateSemaphore.Core;

namespace KarateSemaphore
{
    public class WpfCommandManager : ICommandManager
    {
        public event EventHandler RequerySuggested
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void InvalidateRequerySuggested()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}