using System.Globalization;

namespace KarateSemaphore.Events
{
    public class ChangeDisplayNameSettingsEvent : IEvent
    {
        private readonly ISemaphore _semaphore;
        private readonly IDisplayNameSettings _new;
        private readonly IDisplayNameSettings _old;
        private readonly string _display;
        private class SettingsSnapshot : IDisplayNameSettings
        {
            private readonly string _aka;
            private readonly string _ao;
            public SettingsSnapshot(string aka, string ao)
            {
                _aka = aka;
                _ao = ao;
            }
            public string Aka{get { return _aka; }}
            public string Ao{get { return _ao; }}
        }

        public ChangeDisplayNameSettingsEvent(ISemaphore semaphore, IDisplayNameSettings settings)
        {
            _semaphore = semaphore;
            _new = new SettingsSnapshot(settings.Aka, settings.Ao);
            _old = new SettingsSnapshot( _semaphore.Aka.DisplayName,  _semaphore.Ao.DisplayName);
            _display = string.Format(CultureInfo.InvariantCulture, "Changing display names: '{0}' vs. '{1}'", _new.Aka, _new.Ao);
        }

        public string Display
        {
            get { return _display; }
        }

        public void Redo()
        {
            ApplySettings(_new);
        }

        public void Undo()
        {
            ApplySettings(_old);
        }

        private void ApplySettings(IDisplayNameSettings settings)
        {
            _semaphore.Aka.DisplayName = settings.Aka;
            _semaphore.Ao.DisplayName = settings.Ao;
        }
    }
}