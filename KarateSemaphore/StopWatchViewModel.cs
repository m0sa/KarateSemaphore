#region

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using KarateSemaphore.Core;

#endregion

namespace KarateSemaphore
{
    /// <summary>
    ///   A view model for the stopwatch functionality.
    /// </summary>
    public class StopWatchViewModel : ViewModelBase, IStopWatch
    {
        private readonly RelayCommand<TimeSpan> _delta;
        private readonly RelayCommand<TimeSpan> _reset;
        private readonly RelayCommand _startStop;
        private readonly IDisposable _timeObserver;
        private bool _atoshibaraku;
        private bool _disposed;
        private bool _isStarted;
        private bool _matchEnd;
        private DateTime? _previous;
        private TimeSpan _remaining;

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
                            TimeSpan difference = t - (_previous ?? t);
                            _previous = t;
                            if (_isStarted && difference.Ticks > 0)
                            {
                                _remaining -= difference;
                                if (_remaining <= TimeSpan.Zero)
                                {
                                    _remaining = TimeSpan.Zero;
                                    if (!_matchEnd)
                                    {
                                        _matchEnd = true;
                                        OnMatchEnd();
                                    }
                                }
                                OnPropertyChanged(() => Remaining);
                            }
                            if (_remaining <= TimeSpan.FromSeconds(10) && !_atoshibaraku)
                            {
                                _atoshibaraku = true;
                                OnAtoshibaraku();
                            }
                        });

            _startStop = new RelayCommand(() => { _isStarted = !_isStarted; });

            _reset = new RelayCommand<TimeSpan>(
                t =>
                    {
                        _isStarted = false;
                        _atoshibaraku = false;
                        _matchEnd = false;
                        _remaining = t;
                        OnPropertyChanged(() => Remaining);
                    });

            _delta = new RelayCommand<TimeSpan>(
                t =>
                    {
                        if (_isStarted) return;
                        _matchEnd = false;
                        _remaining = _remaining + t;
                        _atoshibaraku = _remaining > TimeSpan.FromSeconds(10);
                        OnPropertyChanged(() => Remaining);
                        CommandManagerProvider.Instance.InvalidateRequerySuggested();
                    },
                t => !_isStarted);
        }

        #region IStopWatch Members

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

        public ICommand Delta
        {
            get { return _delta; }
        }

        public event EventHandler Atoshibaraku = delegate { };

        public event EventHandler MatchEnd = delegate { };

        #endregion

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
            if (_disposed)
            {
                return;
            }
            Atoshibaraku = null;
            MatchEnd = null;
            if (disposing && _timeObserver != null)
            {
                _timeObserver.Dispose();
            }
            _disposed = true;
        }
    }
}