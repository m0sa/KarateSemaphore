using System;
using WkfSemaphore.Events;

namespace WkfSemaphore
{
    public class SemaphoreViewModel : ViewModelBase
    {
        private readonly CompetitorViewModel _aka;
        private readonly CompetitorViewModel _ao;
        private readonly RelayCommand _reset;
        private readonly StopWatchViewModel _time;
        private readonly EventManagerViewModel _eventManager;
        private TimeSpan _resetTime;

        public SemaphoreViewModel()
        {
            _eventManager = new EventManagerViewModel();
            _aka = new CompetitorViewModel(Belt.Aka, _eventManager);
            _ao = new CompetitorViewModel(Belt.Ao, _eventManager);
            _resetTime = TimeSpan.FromMinutes(3);
            _time = new StopWatchViewModel(_resetTime);
            _reset = new RelayCommand(OnReset);
        }

        public TimeSpan ResetTime
        {
            get { return _resetTime; }
            set
            {
                _resetTime = value;
                OnPropertyChanged("ResetTime");
            }
        }

        public EventManagerViewModel EventManager
        {
            get { return _eventManager; }
        }

        public RelayCommand Reset
        {
            get { return _reset; }
        }

        public StopWatchViewModel Time
        {
            get { return _time; }
        }

        public CompetitorViewModel Aka
        {
            get { return _aka; }
        }

        public CompetitorViewModel Ao
        {
            get { return _ao; }
        }

        private void OnReset()
        {
            _eventManager.Clear();
            _aka.C1 = _aka.C2 = _ao.C1 = _ao.C2 = Penalty.None;
            _aka.Points = _ao.Points = 0;
            
            _time.Reset.Execute(_resetTime);
        }
    }
}