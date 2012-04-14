using System;

using KarateSemaphore.Events;

namespace KarateSemaphore
{
    using System.Reactive.Linq;

    /// <summary>
    /// The base view model used for the controller and display views.
    /// </summary>
    public class SemaphoreViewModel : ViewModelBase
    {
        private readonly CompetitorViewModel _aka;
        private readonly CompetitorViewModel _ao;
        private readonly RelayCommand _reset;
        private readonly StopWatchViewModel _time;
        private readonly EventManagerViewModel _eventManager;
        private TimeSpan _resetTime;

        /// <summary> 
        /// Creates a new instance of the <see cref="SemaphoreViewModel"/> class.
        /// </summary>
        public SemaphoreViewModel(StopWatchViewModel time, EventManagerViewModel eventManager)
        {
            _eventManager = eventManager;
            _aka = new CompetitorViewModel(Belt.Aka, _eventManager);
            _ao = new CompetitorViewModel(Belt.Ao, _eventManager);
            _resetTime = TimeSpan.FromMinutes(3);

            
            _time = time;
            _reset = new RelayCommand(() =>
            {
                _eventManager.Clear();
                _aka.C1 = _aka.C2 = _ao.C1 = _ao.C2 = Penalty.None;
                _aka.Points = _ao.Points = 0;
            
                _time.Reset.Execute(_resetTime);
            });
        }

        /// <summary>
        /// Gets or sets the <see cref="TimeSpan"/> to be used as the argument when the <see cref="Reset"/> 
        /// command is executed. The given value is the forwarded to the <see cref="StopWatchViewModel.Reset"/> 
        /// command.
        /// </summary>
        public TimeSpan ResetTime
        {
            get { return _resetTime; }
            set
            {
                _resetTime = value;
                OnPropertyChanged(() => ResetTime);
            }
        }

        /// <summary>
        /// Gets the view model of the event manager.
        /// </summary>
        public EventManagerViewModel EventManager
        {
            get { return _eventManager; }
        }

        /// <summary>
        /// Gets the command for reseting of the match.
        /// </summary>
        public RelayCommand Reset
        {
            get { return _reset; }
        }

        /// <summary>
        /// Gets the view model of the stopwatch.
        /// </summary>
        public StopWatchViewModel Time
        {
            get { return _time; }
        }

        /// <summary>
        /// Gets the view model for the competitor with the <see cref="Belt.Aka"/> <see cref="Belt"/>.
        /// </summary>
        public CompetitorViewModel Aka
        {
            get { return _aka; }
        }

        /// <summary>
        /// Gets the view model for the competitor with the <see cref="Belt.Ao"/> <see cref="Belt"/>.
        /// </summary>
        public CompetitorViewModel Ao
        {
            get { return _ao; }
        }
    }
}