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
        private readonly ICompetitor _aka;
        private readonly ICompetitor _ao;
        private readonly RelayCommand _reset;
        private readonly IStopWatch _time;
        private readonly IEventManager _eventManager;
        private TimeSpan _resetTime;

        /// <summary>
        ///   Creates a new instance of the <see cref="Semaphore" /> class.
        /// </summary>
        public Semaphore(IStopWatch time, IEventManager eventManager, ICompetitor aka, ICompetitor ao)
        {
            if (time == null) throw new ArgumentNullException("time");
            if (eventManager == null) throw new ArgumentNullException("eventManager");
            if (aka.Belt != Belt.Aka) throw new ArgumentException("Expected Belt.Aka", "aka");
            if (ao.Belt != Belt.Ao) throw new ArgumentException("Expected Belt.Ao", "ao");

            _eventManager = eventManager;
            _aka = aka;
            _ao = ao; 
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

        private static void ResetCompetitor(ICompetitor competitor)
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

        public ICompetitor Aka
        {
            get { return _aka; }
        }

        public ICompetitor Ao
        {
            get { return _ao; }
        }
    }
}