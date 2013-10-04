using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using KarateSemaphore.Core;
using Microsoft.Phone.Controls;

namespace KarateSemaphore.Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

        private ISemaphore Semaphore
        {
            get { return DataContext as ISemaphore; }
        }

        private void InvokeCommand(ICommand command, object parameter = null)
        {
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        private void MenuSettings(object sender, EventArgs e)
        {
        }

        private void MenuKnockdown(object sender, EventArgs e)
        {
            InvokeCommand(Semaphore.ToggleKnockdownMode);
        }

        private void MenuReset(object sender, EventArgs e)
        {
            InvokeCommand(Semaphore.Reset);
        }

        private void MenuUndo(object sender, EventArgs e)
        {
            Semaphore.EventManager.Undo(1);
        }
    }
}