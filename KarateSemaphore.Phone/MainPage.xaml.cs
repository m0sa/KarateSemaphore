using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using KarateSemaphore.Core;
using Microsoft.Advertising;

namespace KarateSemaphore.Phone
{
    public partial class MainPage
    {
        // Constructor
        public MainPage()
        {
            try
            {
                InitializeComponent();
                settingsAd.CountryOrRegion = RegionInfo.CurrentRegion.TwoLetterISORegionName;
                SemaphoreView.IsInversed = App.Settings.FlipOrientation;
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

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            App.Settings.MatchTime = Semaphore.ResetTime;
            App.Settings.FlipOrientation = SemaphoreView.IsInversed;
        }
    }
}