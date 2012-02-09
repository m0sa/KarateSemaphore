using System.Windows;
using System.Windows.Input;

namespace WkfSemaphore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var vm = new SemaphoreViewModel();
            var semaforView = new SemaphoreView { DataContext = vm };
            var semaforControllerView = new SemaphoreControllerView { DataContext = vm };
            semaforView.Show();
            semaforControllerView.Show();
        }
    }
}
