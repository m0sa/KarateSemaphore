using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace KarateSemaphore
{
    /// <summary>
    /// A view model for the stopwatch functionality.
    /// </summary>
    public class StopWatchViewModel : ViewModelBase
    {
        private readonly RelayCommand<TimeSpan> _reset;
        private readonly RelayCommand _startStop;
        private TimeSpan _remaining;

        /// <summary>
        /// Creates a new instance of the <see cref="StopWatchViewModel"/> class.
        /// </summary>
        /// <param name="initial">The initial time set on the stopwatch.</param>
        public StopWatchViewModel(TimeSpan initial)
        {
            _remaining = initial;

            var stopwatch = new Stopwatch(); // measures the time, needs to be more exact
            var timer = new DispatcherTimer(); // updates the GUI, reads the stopwatch
            bool atoshibaraku = false;

            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += (s, e) =>
            {
                TimeSpan nextRemaining = initial - stopwatch.Elapsed;

                // match end
                if (nextRemaining.Ticks <= 0)
                {
                    _remaining = TimeSpan.FromSeconds(0);
                    OnMatchEnd();
                    OnPropertyChanged(() => Remaining);
                    timer.Stop();
                    stopwatch.Stop();
                    return;
                }

                // normal elapsed tick event
                _remaining = nextRemaining;
                OnPropertyChanged(() => Remaining);

                // atoshibaraku logic
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
                OnPropertyChanged(() => Remaining);
            });
        }

        /// <summary>
        /// Gets the command for resetting the stopwatch to an interval given with the command argument.
        /// </summary>
        public RelayCommand<TimeSpan> Reset
        {
            get { return _reset; }
        }

        /// <summary>
        /// Gets the command for toggling the start and paused state of the stopwatch.
        /// </summary>
        public RelayCommand StartStop
        {
            get { return _startStop; }
        }

        /// <summary>
        /// Gets the raining time of the current match.
        /// </summary>
        public TimeSpan Remaining
        {
            get { return _remaining; }
        }

        /// <summary>
        /// Event that happen 10 seconds before the <see cref="MatchEnd"/> event.
        /// </summary>
        public event EventHandler Atoshibaraku = delegate { };

        /// <summary>
        /// Event that signals the end of a match.
        /// </summary>
        public event EventHandler MatchEnd = delegate { };

        /// <summary>
        /// Raises the <see cref="Atoshibaraku"/> event.
        /// </summary>
        private void OnAtoshibaraku()
        {
            Dispatcher.CurrentDispatcher.Invoke(Atoshibaraku, this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="MatchEnd"/> event.
        /// </summary>
        private void OnMatchEnd()
        {
            Dispatcher.CurrentDispatcher.Invoke(MatchEnd, this, EventArgs.Empty);
        }
    }
}