using System.Windows.Input;
using KarateSemaphore.Events;

namespace KarateSemaphore
{
    public class DisplayTextEditorViewModel : ViewModelBase, IDisplayTextSettings
    {
        private readonly ISemaphore _semaphore;
        private readonly RelayCommand _changeDisplayText;
        private string _aka;
        private string _ao;

        public DisplayTextEditorViewModel() {}

        public DisplayTextEditorViewModel(ISemaphore semaphore)
        {
            _semaphore = semaphore;
            _changeDisplayText = new RelayCommand(
                () => _semaphore.EventManager.AddAndExecute(new ChangeDisplayTextSettingsEvent(_semaphore, this)));
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


        public ICommand ChangeDisplayText
        {
            get { return _changeDisplayText; }
        }
    }
}