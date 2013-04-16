#region

using System.Windows.Input;
using KarateSemaphore.Core.Events;

#endregion

namespace KarateSemaphore.Core
{
    /// <summary>
    ///   A view model for representing a competitor in a given match.
    /// </summary>
    public class CompetitorViewModel : ViewModelBase, ICompetitor
    {
        private readonly Belt _belt;
        private readonly IEventManager _eventManager;
        private readonly RelayCommand<Penalty> _changeC1;
        private readonly RelayCommand<Penalty> _changeC2;
        private readonly RelayCommand<Award> _changePoints;
        private Penalty _c1;
        private Penalty _c2;
        private int _points;

        /// <summary>
        ///   Creates a new instance of the <see cref="CompetitorViewModel" /> class.
        /// </summary>
        /// <remarks>
        ///   Parameterless constructor for designtime support.
        /// </remarks>
        public CompetitorViewModel()
            : this(Belt.None, null)
        {
        }

        /// <summary>
        ///   Creates a new instance of the <see cref="CompetitorViewModel" /> class.
        /// </summary>
        /// <param name="belt"> The belt color of the competitor. </param>
        /// <param name="eventManager"> The event manager used for execution of <see cref="Penalty" /> and <see cref="Award" /> events. </param>
        public CompetitorViewModel(Belt belt, IEventManager eventManager)
        {
            _belt = belt;
            _eventManager = eventManager;
            _points = 0;
            _c1 = Penalty.None;
            _c2 = Penalty.None;

            _changePoints = new RelayCommand<Award>(a => _eventManager.AddAndExecute(new AwardEvent(this, a)));
            _changeC1 = new RelayCommand<Penalty>(p => _eventManager.AddAndExecute(new PenaltyEvent(this, p, () => C1)));
            _changeC2 = new RelayCommand<Penalty>(p => _eventManager.AddAndExecute(new PenaltyEvent(this, p, () => C2)));
        }

        public Belt Belt
        {
            get { return _belt; }
        }

        public int Points
        {
            get { return _points; }
            set
            {
                _points = value;
                OnPropertyChanged(() => Points);
            }
        }

        public Penalty C1
        {
            get { return _c1; }
            set
            {
                _c1 = value;
                OnPropertyChanged(() => C1);
            }
        }

        public Penalty C2
        {
            get { return _c2; }
            set
            {
                _c2 = value;
                OnPropertyChanged(() => C2);
            }
        }

        public ICommand ChangePoints
        {
            get { return _changePoints; }
        }

        public ICommand ChangeC1
        {
            get { return _changeC1; }
        }

        public ICommand ChangeC2
        {
            get { return _changeC2; }
        }

        private string _displayText;

        public string DisplayText
        {
            get { return _displayText; }
            set { _displayText = value; OnPropertyChanged(() => DisplayText); }
        }
    }
}