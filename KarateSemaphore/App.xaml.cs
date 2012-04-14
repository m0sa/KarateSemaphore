using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace KarateSemaphore
{
    using System.Reactive.Linq;
    using KarateSemaphore.Events;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private const string TitlePrefix = "Karate Semaphore | ";
        protected override void OnStartup(StartupEventArgs startupArgs)
        {
            base.OnStartup(startupArgs);
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // create media
            var atoshibaraku = CreateAndPreBufferMedia("Media/atoshibaraku.wav");
            var matchEnd = CreateAndPreBufferMedia("Media/end.wav");

            // create view model
            var eventManager = new EventManagerViewModel();
            var time = Observable.Interval(TimeSpan.FromMilliseconds(50)).Select(x => DateTime.Now);
            var stopWatch = new StopWatchViewModel(time);
            stopWatch.Reset.Execute(TimeSpan.FromMinutes(3));
            var vm = new SemaphoreViewModel(stopWatch, eventManager);
            vm.Time.Atoshibaraku += (s, e) => atoshibaraku.Play();
            vm.Time.MatchEnd += (s, e) => matchEnd.Play();

            // create and show windows
            var display = new Window { Content = new SemaphoreView { DataContext = vm } };
            SetupDisplayWindow(display);

            var controller = new Window { Content = new SemaphoreControllerView { DataContext = vm } };
            SetupControllerWindow(controller, vm);
            
            display.Show();
            controller.Show();
        }
        
        private static void SetupDisplayWindow(Window display)
        {
            display.Title = TitlePrefix + "Scoreboard Display";
            var defaultWindowStyle = display.WindowStyle;
            var hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(display).EnsureHandle());

            Action toNormal = () =>
            {
                display.Topmost = false;
                display.WindowStyle = defaultWindowStyle;
                display.WindowState = WindowState.Normal;
            };
            
            hwndSource.AddHook(delegate(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
            {
                const int WM_SYSCOMMAND = 0x112;
                const int SC_MAXIMIZE = 0xF030;
                const int SC_CLOSE = 0xF060;
                const int SC_RESTORE = 0xF120;
                if (msg == WM_SYSCOMMAND)
                {
                    var sysCommand = wParam.ToInt32();
                    switch (sysCommand)
                    {
                        case SC_MAXIMIZE:
                            display.Topmost = true;
                            display.WindowStyle = WindowStyle.None;
                            break;
                        case SC_CLOSE:
                            toNormal();
                            handled = true;
                            break;
                        case SC_RESTORE:
                            toNormal();
                            break;
                    }
                }
                return IntPtr.Zero;
            });
        }

        private void SetupControllerWindow(Window controller, SemaphoreViewModel vm)
        {
            controller.Title = TitlePrefix + "Scoreboard Controller";
            controller.PreviewKeyDown += (s, e) => HandlePreviewKey(controller, e);
            controller.InputBindings.Add(new KeyBinding(vm.Time.StartStop, new KeyGesture(Key.Space)));
            controller.InputBindings.Add(new KeyBinding(vm.Reset, new KeyGesture(Key.F9)));
            controller.PreviewMouseDown += (s, e) => HandleFocus(controller);
            controller.Closed += (s, e) => Shutdown(0);
        }

        private static void HandlePreviewKey(UIElement controller, KeyEventArgs keyEventArgs)
        {
            // handle focus change if any of the shourtcut key is about to be pressed...
            if (controller.InputBindings.OfType<KeyBinding>().Any(x => x.Key == keyEventArgs.Key))
            {
                HandleFocus(controller);
            }
        }

        private static void HandleFocus(IInputElement window)
        {
            // Prevents that UI elements get keyboard focus and disable the shortcut key command bindings.
            // Prevents focusing of all controlls (except textboxes).
            // Fixes the textbox behavior where a textbox keeps keyboard focus if a non focusable button is clicked.
            var inputElement = Keyboard.FocusedElement as UIElement;
            if (inputElement == null || inputElement == window) return;

            inputElement.Focusable = false;

            Keyboard.ClearFocus();
            window.Focus(); // Focus on the window instead

            if (!(inputElement is TextBox)) return;
            // The textbox must still be focusable if we want to type d00h...
            inputElement.Focusable = true;
            // Fixes the updating of a source bound to a textbox text property.
            var textBinding = BindingOperations.GetBindingExpressionBase(inputElement,
                                                                                           TextBox.TextProperty);
            if (textBinding != null) textBinding.UpdateSource();
        }


        private static MediaPlayer CreateAndPreBufferMedia(string uri)
        {
            var mplayer = new MediaPlayer();
            mplayer.MediaFailed += (s, e) => Console.WriteLine(e.ErrorException.ToString());
            mplayer.Open(new Uri("pack://siteoforigin:,,,/" + uri));
            mplayer.Volume = 0.0; // play once with volume
            mplayer.Play(); // prebuffer with volume 0
            mplayer.MediaEnded += (s, e) =>
            {
                mplayer.Stop();
                mplayer.Volume = 1.0;
                mplayer.Position = TimeSpan.Zero;
            };
            return mplayer;
        }
    }
}