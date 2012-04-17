#region

using System;
using System.Windows.Input;
using KarateSemaphore.Events;

#endregion

namespace KarateSemaphore
{
    /// <summary>
    ///   The base view model used for the controller and display views.
    /// </summary>
    public class Semaphore : ViewModelBase, ISemaphore
    {
        private readonly CompetitorViewModel _aka;
        private readonly CompetitorViewModel _ao;
        private readonly RelayCommand _reset;
        private readonly IStopWatch _time;
        private readonly IEventManager _eventManager;
        private TimeSpan _resetTime;

        /// <summary>
        ///   Creates a new instance of the <see cref="Semaphore" /> class.
        /// </summary>
        public Semaphore(IStopWatch time, IEventManager eventManager)
        {
            _eventManager = eventManager;
            _aka = new CompetitorViewModel(Belt.Aka, _eventManager);
            _ao = new CompetitorViewModel(Belt.Ao, _eventManager);
            _resetTime = TimeSpan.FromMinutes(3);


            _time = time;
            _reset = new RelayCommand(
                () =>
                {
                    ResetCompetitor(_aka);
                    ResetCompetitor(_ao);

                    _time.Reset.Execute(_resetTime);
                    _eventManager.Clear();
                });
        }

        private static void ResetCompetitor(CompetitorViewModel competitor)
        {
            competitor.ChangeC1.Execute(Penalty.None);
            competitor.ChangeC2.Execute(Penalty.None);
            competitor.ChangePoints.Execute(Award.None);
        }

        public TimeSpan ResetTime
        {
            get { return _resetTime; }
            set
            {
                _resetTime = value;
                OnPropertyChanged(() => ResetTime);
            }
        }

        public IEventManager EventManager
        {
            get { return _eventManager; }
        }

        public ICommand Reset
        {
            get { return _reset; }
        }

        public IStopWatch Time
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
    }
}