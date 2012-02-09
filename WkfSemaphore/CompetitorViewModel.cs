using WkfSemaphore.Events;

namespace WkfSemaphore
{
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

        public CompetitorViewModel(Belt belt = Belt.None, IEventManager eventManager = null)
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

        public RelayCommand<Award> ChangePoints
        {
            get { return _changePoints; }
        }

        public RelayCommand<Penalty> ChangeC1
        {
            get { return _changeC1; }
        }

        public RelayCommand<Penalty> ChangeC2
        {
            get { return _changeC2; }
        }
    }
}