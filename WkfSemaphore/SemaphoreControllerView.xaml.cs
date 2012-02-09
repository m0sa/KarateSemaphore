using System.Windows;
using System.Windows.Input;

namespace WkfSemaphore
{
    /// <summary>
    /// Interaction logic for SemaphoreControllerView.xaml
    /// </summary>
    public partial class SemaphoreControllerView : Window
    {
        public SemaphoreControllerView()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {

            var viewModel = (SemaphoreViewModel)this.DataContext;
            
            this.InputBindings.Add(new KeyBinding(viewModel.Time.StartStop, new KeyGesture(Key.Space)));
            this.InputBindings.Add(new KeyBinding(viewModel.Reset, new KeyGesture(Key.F9)));
        }
    }
}
