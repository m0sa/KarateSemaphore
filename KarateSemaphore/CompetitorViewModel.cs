using KarateSemaphore.Events;

namespace KarateSemaphore
{
    /// <summary>
    /// A view model for representing a competitor in a given match.
    /// </summary>
    public class CompetitorViewModel : ViewModelBase
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
        /// Creates a new instance of the <see cref="CompetitorViewModel"/> class.
        /// </summary>
        /// <remarks>
        /// Parameterless constructor for designtime support.
        /// </remarks>
        public CompetitorViewModel()
            : this(Belt.None, null)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CompetitorViewModel"/> class.
        /// </summary>
        /// <param name="belt">The belt color of the competitor.</param>
        /// <param name="eventManager">The event manager used for execution of <see cref="Penalty"/> and <see cref="Award"/> events.</param>
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

        /// <summary>
        /// Gets the <see cref="Belt"/> color.
        /// </summary>
        public Belt Belt
        {
            get { return _belt; }
        }

        /// <summary>
        /// Gets or sets the total points.
        /// </summary>
        public int Points
        {
            get { return _points; }
            set
            {
                _points = value;
                OnPropertyChanged(() => Points);
            }
        }

        /// <summary>
        /// Gets the penalties in the category C1.
        /// </summary>
        public Penalty C1
        {
            get { return _c1; }
            set
            {
                _c1 = value;
                OnPropertyChanged(() => C1);
            }
        }

        /// <summary>
        /// Gets the penalties in the category C2.
        /// </summary>
        public Penalty C2
        {
            get { return _c2; }
            set
            {
                _c2 = value;
                OnPropertyChanged(() => C2);
            }
        }

        /// <summary>
        /// Gets the command that can be used to <see cref="Award"/> the competitor with points.
        /// </summary>
        public RelayCommand<Award> ChangePoints
        {
            get { return _changePoints; }
        }

        /// <summary>
        /// Gets the command that can be used to issue an <see cref="Penalty"/> in the category C1.
        /// </summary>
        public RelayCommand<Penalty> ChangeC1
        {
            get { return _changeC1; }
        }

        /// <summary>
        /// Gets the command that can be used to issue an <see cref="Penalty"/> in the category C2.
        /// </summary>
        public RelayCommand<Penalty> ChangeC2
        {
            get { return _changeC2; }
        }
    }
}