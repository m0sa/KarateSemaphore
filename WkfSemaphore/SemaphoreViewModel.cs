using System;

namespace WkfSemaphore
{
    public class SemaphoreViewModel : ViewModelBase
    {
        private readonly CompetitorViewModel _aka;
        private readonly CompetitorViewModel _ao;
        private readonly RelayCommand _reset;
        private readonly StopWatchViewModel _time;
        private TimeSpan _resetTime;

        public SemaphoreViewModel()
        {
            _aka = new CompetitorViewModel();
            _ao = new CompetitorViewModel();
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
            _aka.ChangeC1.Execute(Penalty.None);
            _aka.ChangeC2.Execute(Penalty.None);
            _aka.ChangePoints.Execute(_aka.Points*-1);

            _ao.ChangeC1.Execute(Penalty.None);
            _ao.ChangeC2.Execute(Penalty.None);
            _ao.ChangePoints.Execute(_ao.Points*-1);

            _time.Reset.Execute(_resetTime);
        }
    }
}