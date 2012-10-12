using System;

namespace KarateSemaphore
{
    public class ApplicationSettings : ISettings
    {
        private readonly Properties.Settings _settings;
        public ApplicationSettings()
        {
            _settings = Properties.Settings.Default;
            _settings.Upgrade();
        }

        public void PersistChanges()
        {
            _settings.Save();
            OnChanged();
        }

        public void DiscardChanges()
        {
            _settings.Reload();
            OnChanged();
        }

        public string SoundAtoshibaraku
        {
            get { return _settings.SoundAtoshibaraku; } 
            set { _settings.SoundAtoshibaraku = value; }
        }

        public string SoundMatchEnd
        {
            get { return _settings.SoundMatchEnd; }
            set { _settings.SoundMatchEnd = value; }
        }

        protected virtual void OnChanged()
        {
            var handler = Changed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public event EventHandler Changed;
    }
}