using System.Windows;

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
            var semaforView = new SemaforView();
            semaforView.DataContext = vm;
            var semaforControllerView = new SemaforControllerView();
            semaforControllerView.DataContext = vm;
            semaforView.Show();
            semaforControllerView.Show();
        }
    }
}
