using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using KarateSemaphore.Core;
using Microsoft.Advertising;
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
                settingsAd.CountryOrRegion = RegionInfo.CurrentRegion.TwoLetterISORegionName;
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
            SettingsPopup.Visibility = SettingsPopup.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
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

        private void AdControl_OnErrorOccurred(object sender, AdErrorEventArgs e)
        {
        }
    }

    public class SettingsViewModel : DependencyObject
    {

    }
}