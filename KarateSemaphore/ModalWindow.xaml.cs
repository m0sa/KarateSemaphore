using System.Windows;
using System.Windows.Input;

namespace KarateSemaphore
{
    /// <summary>
    /// Interaction logic for ModalWindow.xaml
    /// </summary>
    public partial class ModalWindow
    {
        
        public ModalWindow()
        {
            InitializeComponent();
        }

        public ModalWindow(UIElement view, object viewModel, ICommand okCommand) : this()
        {
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ShowInTaskbar = true;

            theGrid.Children.Add(view);
            DataContext = viewModel;
            okButton.Command = okCommand;
            okButton.Click += CloseDialog;
            cancelButton.Click += CloseDialog;
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
