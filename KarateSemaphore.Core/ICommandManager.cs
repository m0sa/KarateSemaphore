using System;

namespace KarateSemaphore.Core
{
    public interface ICommandManager {
        event EventHandler RequerySuggested;
        void InvalidateRequerySuggested();
    }
}