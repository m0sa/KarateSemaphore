using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.Phone;

namespace KarateSemaphore.Phone
{
    public partial class PenaltyDisplayItem
    {

        public static readonly DependencyProperty ActiveBrushProperty =
            DependencyProperty.Register("ActiveBrush", typeof(Brush), typeof(PenaltyDisplayItem), new PropertyMetadata(new SolidColorBrush(Colors.Magenta)));

        public static readonly DependencyProperty InactiveBrushProperty =
            DependencyProperty.Register("ActiveBrush", typeof(Brush), typeof(PenaltyDisplayItem), new PropertyMetadata(new SolidColorBrush(Colors.Cyan)));
        
        public Brush ActiveBrush {
            get { return (Brush)GetValue(ActiveBrushProperty); }
            set { SetValue(ActiveBrushProperty, value); }
        }

        public Brush InactiveBrush {
            get { return (Brush)GetValue(InactiveBrushProperty); }
            set { SetValue(InactiveBrushProperty, value); }
        }
        
        public PenaltyDisplayItem() {
            IsEnabledChanged += OnIsEnabledChanged;
            InitializeComponent();
            UpdateBackground();
        }

        private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
            UpdateBackground();
        }

        private void UpdateBackground() {
            //SetBinding(BackgroundProperty, new Binding {Path = new PropertyPath(IsEnabled ? ActiveBrushProperty : InactiveBrushProperty)});
            Background = IsEnabled ? ActiveBrush : InactiveBrush;
        }
    }
}
