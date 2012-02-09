namespace WkfSemaphore
{
    public class CompetitorViewModel : ViewModelBase
    {
        private readonly RelayCommand<Penalty> _changeC1;
        private readonly RelayCommand<Penalty> _changeC2;
        private readonly RelayCommand<int> _changePoints;
        private Penalty _c1;
        private Penalty _c2;
        private int _points;

        public CompetitorViewModel()
        {
            _changePoints = new RelayCommand<int>(OnChangePoints);
            _changeC1 = new RelayCommand<Penalty>(OnChangeC1);
            _changeC2 = new RelayCommand<Penalty>(OnChangeC2);
            _points = 0;
            _c1 = Penalty.None;
            _c2 = Penalty.None;
        }

        public int Points
        {
            get { return _points; }
        }

        public Penalty C1
        {
            get { return _c1; }
        }

        public Penalty C2
        {
            get { return _c2; }
        }

        public RelayCommand<int> ChangePoints
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

        private void OnChangePoints(int change)
        {
            int newPoints = _points + change;
            if (newPoints >= 0)
            {
                _points = newPoints;
                OnPropertyChanged("Points");
            }
        }

        private void OnChangeC1(Penalty change)
        {
            _c1 = change;
            OnPropertyChanged("C1");
        }

        private void OnChangeC2(Penalty change)
        {
            _c2 = change;
            OnPropertyChanged("C2");
        }
    }
}