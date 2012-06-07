using System;
using System.Globalization;

namespace KarateSemaphore.Events
{
    public class ChangeDisplayTextSettingsEvent : IEvent
    {
        private readonly ISemaphore _semaphore;
        private readonly IDisplayTextSettings _new;
        private readonly IDisplayTextSettings _old;
        private readonly string _display;
        private class SettingsSnapshot : IDisplayTextSettings
        {
            private readonly string _aka;
            private readonly string _ao;
            public SettingsSnapshot(string aka, string ao)
            {
                _aka = (aka ?? string.Empty).Trim();
                _ao = (ao ?? string.Empty).Trim();
            }
            public string Aka { get { return _aka; } }
            public string Ao { get { return _ao; } }
        }

        public ChangeDisplayTextSettingsEvent(ISemaphore semaphore, IDisplayTextSettings settings)
        {
            _semaphore = semaphore;
            _new = new SettingsSnapshot(settings.Aka, settings.Ao);
            _old = new SettingsSnapshot(_semaphore.Aka.DisplayText, _semaphore.Ao.DisplayText);
            _display = string.Concat("Aka text:", Environment.NewLine, _new.Aka, Environment.NewLine, "Ao text:", Environment.NewLine, _new.Ao);
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

        private void ApplySettings(IDisplayTextSettings settings)
        {
            _semaphore.Aka.DisplayText = settings.Aka;
            _semaphore.Ao.DisplayText = settings.Ao;
        }
    }
}