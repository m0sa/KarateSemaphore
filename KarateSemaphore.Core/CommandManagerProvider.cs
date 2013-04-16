using System;

namespace KarateSemaphore.Core
{
    public static class CommandManagerProvider
    {
        private static ICommandManager _instance = new EmptyCommandManagerProvider();
        public static ICommandManager Instance {
            get { return _instance; }
            set { _instance = value; }
        }

        private class EmptyCommandManagerProvider : ICommandManager {
            public event EventHandler RequerySuggested;
            public void InvalidateRequerySuggested() {
                throw new NotImplementedException();
            }
        }
    }
}