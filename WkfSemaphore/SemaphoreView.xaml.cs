using System;
using System.Windows;
using System.Windows.Media;

namespace WkfSemaphore
{
    /// <summary>
    /// Interaction logic for SemaphoreView.xaml
    /// </summary>
    public partial class SemaphoreView : Window
    {
        public SemaphoreView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs re)
        {
            var viewModel = (SemaphoreViewModel)this.DataContext;

            viewModel.Time.Atoshibaraku += (s, e) => Play("Media/atoshibaraku.wav");
            viewModel.Time.MatchEnd += (s, e) => Play("Media/end.wav");
        }

        private void Play(string uri)
        {            
            var mplayer = new MediaPlayer();
            mplayer.MediaFailed += (s, e) => Console.WriteLine(e.ErrorException.ToString());
            mplayer.MediaEnded += (s, e) => mplayer.Close();
            mplayer.Open(new Uri("pack://siteoforigin:,,,/" + uri));
            mplayer.Volume = 1.0;
            mplayer.Play();
        }

        private void FullScreenButtonClick(object sender, RoutedEventArgs e)
        {
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            FullScreenButton.Visibility = Visibility.Hidden;
            Topmost = true;
        }

        private void WindowStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                FullScreenButton.Visibility = Visibility.Visible;
                Topmost = false;
            }
        }
    }
}
