using System.Windows.Input;
using KarateSemaphore.Events;

namespace KarateSemaphore
{
    public class DisplayNameEditorViewModel : ViewModelBase, IDisplayNameSettings
    {
        private readonly ISemaphore _semaphore;
        private readonly RelayCommand _changeDisplayNames;
        private string _aka;
        private string _ao;

        public DisplayNameEditorViewModel() {}

        public DisplayNameEditorViewModel(ISemaphore semaphore)
        {
            _semaphore = semaphore;
            _changeDisplayNames = new RelayCommand(
                () => _semaphore.EventManager.AddAndExecute(new ChangeDisplayNameSettingsEvent(_semaphore, this)));
        }

        public string Aka
        {
            get { return _aka; }
            set { _aka = value; OnPropertyChanged(() => Aka); }
        }


        public string Ao
        {
            get { return _ao; }
            set { _ao = value; OnPropertyChanged(() => Ao); }
        }


        public ICommand ChangeDisplayNames
        {
            get { return _changeDisplayNames; }
        }
    }
}