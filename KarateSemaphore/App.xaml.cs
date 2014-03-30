#region

using System;
using System.Deployment.Application;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using KarateSemaphore.Core;
using KarateSemaphore.Core.Events;

#endregion

namespace KarateSemaphore
{
    /// <summary>
    ///   Interaction logic for App.xaml
    /// </summary>
    public partial class App : IModalDialogManager
    {
        private const string TitlePrefix = "Karate Semaphore | ";

        protected override void OnStartup(StartupEventArgs startupArgs)
        {
            base.OnStartup(startupArgs);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            
            // create view model
            // TODO replace with an IoC container.
            var settings = new ApplicationSettings();
            var eventManager = new EventManagerViewModel();
            var time = Observable.Interval(TimeSpan.FromMilliseconds(50)).Select(x => DateTime.Now);
            var commandManager = new WpfCommandManager();
            var stopWatch = new StopWatchViewModel(time, commandManager);
            stopWatch.Reset.Execute(TimeSpan.FromMinutes(3));
            var aka = new CompetitorViewModel(Belt.Aka, eventManager);
            var ao = new CompetitorViewModel(Belt.Ao, eventManager);
            var vm = new SemaphoreViewModel(stopWatch, eventManager, this, aka, ao);
            var applicationInfo = ApplicationDeployment.IsNetworkDeployed
                                      ? (IApplicationInfo) new ClickOnceApplicationInfo()
                                      : new AssemblyBasedApplicationInfo();

            // create and show windows
            var display = new DisplayWindow(vm);
            display.Title = TitlePrefix + "Scoreboard Display";

            var controller = new ControllerWindow(this, settings, vm, applicationInfo);
            controller.Title = TitlePrefix + "Scoreboard Controller";
            controller.Closed += (s, e) => Shutdown(0);

            MainWindow = controller;
            display.Show();
            controller.Show();
        }

        public void ShowDialog<T>(T viewModel, Func<T, ICommand> okAction)
        {
            if (viewModel is DisplayTextEditorViewModel)
            {
                new ModalWindow(new DisplayTextEditorView(), viewModel, okAction(viewModel))
                    {
                        Title =  TitlePrefix + "Display Text Editor",
                        WindowStyle = WindowStyle.ToolWindow,
                        Height = 150
                    }.ShowDialog();
            }
            else if(viewModel is IApplicationInfo)
            {
                new ModalWindow(new AboutBox(), viewModel, okAction(viewModel))
                    {
                        Title = TitlePrefix + "About",
                        WindowStyle = WindowStyle.ToolWindow,
                        Height = 300,
                        Width = 350
                    }.ShowDialog();
            }
        }
    }
}