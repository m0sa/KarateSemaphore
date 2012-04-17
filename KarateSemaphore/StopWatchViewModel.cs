#region

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Windows.Threading;

#endregion

namespace KarateSemaphore
{
    /// <summary>
    ///   A view model for the stopwatch functionality.
    /// </summary>
    public class StopWatchViewModel : ViewModelBase, IStopWatch
    {
        private readonly RelayCommand<TimeSpan> _reset;
        private readonly RelayCommand _startStop;
        private readonly IDisposable _timeObserver;
        private TimeSpan _remaining;
        private DateTime? previous;
        private bool disposed;
        private bool isStarted;
        private bool atoshibaraku;
        private bool matchEnd;

        /// <summary>
        ///   Creates a new instance of the <see cref="StopWatchViewModel" /> class.
        /// </summary>
        /// <param name="time"> An source of time events. </param>
        public StopWatchViewModel(IObservable<DateTime> time)
        {
            _timeObserver = time
                .ObserveOn(Scheduler.CurrentThread) // observe on the GUI thread!
                .Subscribe(
                    t =>
                    {
                        var difference = t - (previous ?? t);
                        previous = t;
                        if (isStarted && difference.Ticks > 0)
                        {
                            _remaining -= difference;
                            if (_remaining <= TimeSpan.Zero)
                            {
                                _remaining = TimeSpan.Zero;
                                if (!matchEnd)
                                {
                                    matchEnd = true;
                                    OnMatchEnd();
                                }
                            }
                            OnPropertyChanged(() => Remaining);
                        }
                        if (_remaining <= TimeSpan.FromSeconds(10) && !atoshibaraku)
                        {
                            atoshibaraku = true;
                            OnAtoshibaraku();
                        }
                    });

            _startStop = new RelayCommand(() => { isStarted = !isStarted; });

            _reset = new RelayCommand<TimeSpan>(
                t =>
                {
                    isStarted = false;
                    atoshibaraku = false;
                    matchEnd = false;
                    _remaining = t;
                    OnPropertyChanged(() => Remaining);
                });
        }

        public ICommand Reset
        {
            get { return _reset; }
        }

        public ICommand StartStop
        {
            get { return _startStop; }
        }

        public TimeSpan Remaining
        {
            get { return _remaining; }
        }

        public event EventHandler Atoshibaraku = delegate { };

        public event EventHandler MatchEnd = delegate { };

        /// <summary>
        ///   Raises the <see cref="Atoshibaraku" /> event.
        /// </summary>
        private void OnAtoshibaraku()
        {
            Dispatcher.CurrentDispatcher.Invoke(Atoshibaraku, this, EventArgs.Empty);
        }

        /// <summary>
        ///   Raises the <see cref="MatchEnd" /> event.
        /// </summary>
        private void OnMatchEnd()
        {
            Dispatcher.CurrentDispatcher.Invoke(MatchEnd, this, EventArgs.Empty);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposed)
            {
                return;
            }
            Atoshibaraku = null;
            MatchEnd = null;
            if (disposing && _timeObserver != null)
            {
                _timeObserver.Dispose();
            }
            disposed = true;
        }
    }
}