using System;
using System.Windows;
using System.Windows.Media;

namespace WkfSemaphore
{
    /// <summary>
    /// Interaction logic for SemaforView.xaml
    /// </summary>
    public partial class SemaforView : Window
    {
        public SemaforView()
        {
            InitializeComponent();
        }
        
        private void CompetitorSemaforView_Loaded(object sender, RoutedEventArgs re)
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

        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.FullScreenButton.Visibility = Visibility.Hidden;
            this.Topmost = true;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.FullScreenButton.Visibility = Visibility.Visible;
                this.Topmost = false;
            }
        }
    }
}
