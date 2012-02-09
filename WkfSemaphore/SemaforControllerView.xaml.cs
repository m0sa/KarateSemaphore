using System.Windows;
using System.Windows.Input;

namespace WkfSemaphore
{
    /// <summary>
    /// Interaction logic for SemaforControllerView.xaml
    /// </summary>
    public partial class SemaforControllerView : Window
    {
        public SemaforControllerView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var viewModel = (SemaphoreViewModel)this.DataContext;
            
            this.InputBindings.Add(new KeyBinding(viewModel.Time.StartStop, new KeyGesture(Key.Space)));
            this.InputBindings.Add(new KeyBinding(viewModel.Reset, new KeyGesture(Key.F9)));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
