using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace KarateSemaphore
{
    public class StopWatchViewModel : ViewModelBase
    {
        private readonly RelayCommand<TimeSpan> _reset;
        private readonly RelayCommand _startStop;
        private TimeSpan _remaining;

        public StopWatchViewModel(TimeSpan initial)
        {
            _remaining = initial;

            var stopwatch = new Stopwatch();
            var timer = new DispatcherTimer();
            bool atoshibaraku = false;

            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += (s, e) =>
            {
                TimeSpan nextRemaining = initial - stopwatch.Elapsed;
                if (nextRemaining.Ticks <= 0)
                {
                    _remaining = TimeSpan.FromSeconds(0);
                    OnMatchEnd();
                    OnPropertyChanged("Remaining");
                    timer.Stop();
                    stopwatch.Stop();
                    return;
                }
                _remaining = nextRemaining;
                OnPropertyChanged("Remaining");
                if (nextRemaining.TotalSeconds < 10 && !atoshibaraku)
                {
                    atoshibaraku = true;
                    OnAtoshibaraku();
                }
            };

            _startStop = new RelayCommand(() =>
            {
                if (timer.IsEnabled)
                {
                    stopwatch.Stop();
                    timer.Stop();
                }
                else
                {
                    stopwatch.Start();
                    timer.Start();
                }
            });

            _reset = new RelayCommand<TimeSpan>(t =>
            {
                timer.Stop();
                stopwatch.Stop();
                stopwatch.Reset();
                initial = t;
                atoshibaraku = false;
                _remaining = t;
                OnPropertyChanged("Remaining");
            });
        }


        public RelayCommand<TimeSpan> Reset
        {
            get { return _reset; }
        }

        public RelayCommand StartStop
        {
            get { return _startStop; }
        }

        public TimeSpan Remaining
        {
            get { return _remaining; }
        }

        public event EventHandler Atoshibaraku = delegate { };
        public event EventHandler MatchEnd = delegate { };

        private void OnAtoshibaraku()
        {
            Dispatcher.CurrentDispatcher.Invoke(Atoshibaraku, this, EventArgs.Empty);
        }

        private void OnMatchEnd()
        {
            Dispatcher.CurrentDispatcher.Invoke(MatchEnd, this, EventArgs.Empty);
        }
    }
}