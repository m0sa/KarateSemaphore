using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using KarateSemaphore.Core;
using KarateSemaphore.Core.Events;
using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace KarateSemaphore.Phone
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                // Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }
        
        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
                if (e.Exception != null)
                {
                    System.Diagnostics.Debug.WriteLine(e.Exception + "");
                    e.Handled = true;
                }
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
                System.Diagnostics.Debug.WriteLine(e.ExceptionObject + "");
                e.Handled = true;
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            Initialize();

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        public static readonly Settings Settings = new Settings();

        #endregion

        private void Initialize()
        {
            var eventManager = new EventManagerViewModel();
            var time = Observable.Interval(TimeSpan.FromMilliseconds(142)).Select(x => DateTime.Now);
            var stopWatch = new StopWatchViewModel(time);
            var aka = new CompetitorViewModel(Belt.Aka, eventManager);
            var ao = new CompetitorViewModel(Belt.Ao, eventManager);
            var vm = new SemaphoreViewModel(stopWatch, eventManager, null, aka, ao);
            vm.ResetTime = Settings.MatchTime;
            stopWatch.Reset.Execute(vm.ResetTime);
            var atoshibaraku = CreatePlayer("Media/atoshibaraku.wav");
            stopWatch.Atoshibaraku += (s, args) =>
            {
                atoshibaraku();
                VibrateController.Default.Start(TimeSpan.FromMilliseconds(200));
            };
            var matchEnd = CreatePlayer("Media/end.wav");
            stopWatch.MatchEnd += (s, args) =>
            {
                matchEnd();
                VibrateController.Default.Start(TimeSpan.FromMilliseconds(1500));
            };
            RootFrame.DataContext = vm;
        }

        private static Action CreatePlayer(string uri)
        {
            using (var stream = TitleContainer.OpenStream(uri))
            {
                var effect = SoundEffect.FromStream(stream);
                FrameworkDispatcher.Update();
                effect.Play(0,0,0); // prebuffer with volume 0
                return () => effect.Play();
            }
        }
    }
}