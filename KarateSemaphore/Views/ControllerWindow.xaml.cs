using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using KarateSemaphore.Core;

namespace KarateSemaphore
{
    /// <summary>
    /// Interaction logic for ControllerWindow.xaml
    /// </summary>
    public partial class ControllerWindow
    {
        private readonly IModalDialogManager _modalDialogManager;
        private readonly ISettings _settings;
        private readonly ISemaphore _semaphore;
        private readonly IApplicationInfo _applicationInfo;
        private Action _atoshibaraku = delegate { };
        private Action _matchEnd = delegate { };
        
        public ControllerWindow(IModalDialogManager modalDialogManager, ISettings settings, ISemaphore semaphore, IApplicationInfo applicationInfo)
        {
            _modalDialogManager = modalDialogManager;
            _settings = settings;
            _semaphore = semaphore;
            _applicationInfo = applicationInfo;
            _semaphore.Time.Atoshibaraku += (s, e) => Dispatcher.Invoke(_atoshibaraku);
            _semaphore.Time.MatchEnd += (s, e) => Dispatcher.Invoke(_matchEnd);

            InitializeComponent();
            Controller.DataContext = _semaphore;

            PreviewKeyDown += HandlePreviewKey;
            PreviewMouseDown += HandleFocus;

            UpdateInputBindings();
            UpdateSounds();
        }

        private void UpdateSounds()
        {
            _atoshibaraku = CreatePlayer(_settings.SoundAtoshibaraku);
            _matchEnd = CreatePlayer(_settings.SoundMatchEnd);
        }

        public void UpdateInputBindings()
        {
            InputBindings.Clear();

            // TODO read from settings
            InputBindings.Add(new KeyBinding(_semaphore.Time.StartStop, new KeyGesture(Key.Space)));
            InputBindings.Add(new KeyBinding(_semaphore.Reset, new KeyGesture(Key.F9)));
            InputBindings.Add(new KeyBinding(_semaphore.ToggleKnockdownMode, new KeyGesture(Key.F10)));
        }

        private static Action CreatePlayer(string uri)
        {
            var mplayer = new MediaPlayer();
            mplayer.MediaFailed += (s, e) => Console.WriteLine(e.ErrorException.ToString());
            mplayer.Open(new Uri(uri));
            mplayer.Volume = 0.0; // play once with volume
            mplayer.Play(); // prebuffer with volume 0

            return () =>
            {
                mplayer.Position = TimeSpan.Zero;
                mplayer.Volume = 1.0;
                mplayer.Play();
            };
        }
        
        private void HandlePreviewKey(object sender, KeyEventArgs args)
        {
            // handle focus change if any of the shourtcut key is about to be pressed...
            if (InputBindings.OfType<KeyBinding>().Any(x => x.Key == args.Key))
            {
                HandleFocus(sender, args);
            }
        }

        private void HandleFocus(object sender, EventArgs args)
        {
            // Prevents that UI elements get keyboard focus and disable the shortcut key command bindings.
            // Prevents focusing of all controlls (except textboxes).
            // Fixes the textbox behavior where a textbox keeps keyboard focus if a non focusable button is clicked.
            var inputElement = Keyboard.FocusedElement as UIElement;
            if (inputElement == null || inputElement is MenuItem || Equals(inputElement))
            {
                return;
            }

            inputElement.Focusable = false;

            Keyboard.ClearFocus();
            Focus(); // Focus on the window instead

            if (!(inputElement is TextBox))
            {
                return;
            }
            // The textbox must still be focusable if we want to type d00h...
            inputElement.Focusable = true;
            // Fixes the updating of a source bound to a textbox text property.
            var textBinding = BindingOperations.GetBindingExpressionBase(
                inputElement,
                TextBox.TextProperty);
            if (textBinding != null)
            {
                textBinding.UpdateSource();
            }
        }

        private static readonly ICommand aboutBoxHandler = new RelayCommand<IApplicationInfo>(_ => { });
        
        private void Menu_AboutBox_Click(object sender, RoutedEventArgs e)
        {
            _modalDialogManager.ShowDialog(_applicationInfo, _ => aboutBoxHandler);
        }

        private void Menu_Homepage_Click(object sender, RoutedEventArgs e)
        {
            var proc = new Process();
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.FileName = "http://karatesemaphore.codeplex.com";
            proc.Start();
        }
    }
}
