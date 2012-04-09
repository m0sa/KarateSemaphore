using System.Windows.Controls;

namespace KarateSemaphore
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for SemaphoreView.xaml
    /// </summary>
    public partial class SemaphoreView : UserControl
    {
        /// <summary>
        /// Gets or sets a value definig whether the aka and ao sides should be inversed.
        /// </summary>
        public bool IsInversed
        {
            get { return (bool)GetValue(IsInversedProperty); }
            set { SetValue(IsInversedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsInversed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsInversedProperty =
            DependencyProperty.Register("IsInversed", typeof(bool), typeof(SemaphoreView), new UIPropertyMetadata(false, IsInversedChanged));

        private static void IsInversedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var semaphoreView = d as SemaphoreView;
            if (semaphoreView == null) return;
            semaphoreView.previewNormal.Visibility = semaphoreView.IsInversed ? Visibility.Hidden : Visibility.Visible;
            semaphoreView.previewInverse.Visibility = semaphoreView.IsInversed ? Visibility.Visible : Visibility.Hidden;
        }

        public SemaphoreView()
        {
            InitializeComponent();
        }
    }
}
