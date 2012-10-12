using System;

namespace KarateSemaphore
{
    public interface ISettings
    {
        void PersistChanges();
        void DiscardChanges();
        string SoundAtoshibaraku { get; set; }
        string SoundMatchEnd { get; set; }
        event EventHandler Changed;
    }
}
