using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Shapes;
using KarateSemaphore.Core;
using KarateSemaphore.Core.Events;
using Microsoft.Phone.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace KarateSemaphore.Phone
{
    /// <summary>
    /// Interaction logic for PenaltyDisplayControl.xaml
    /// </summary>
    public partial class PenaltyDisplayControl : UserControl
    {

        public Penalty Penalty
        {
            get { return (Penalty)GetValue(PenaltyProperty); }
            set { SetValue(PenaltyProperty, value); }
        }

        public static readonly DependencyProperty PenaltyProperty = DependencyProperty.Register("Penalty", typeof(Penalty), typeof(PenaltyDisplayControl), new PropertyMetadata(Penalty.None));

        public bool LTR
        {
            get { return (bool)GetValue(LTRProperty); }
            set { SetValue(LTRProperty, value); }
        }

        public static readonly DependencyProperty LTRProperty =
            DependencyProperty.Register("LTR", typeof(bool), typeof(PenaltyDisplayControl), new PropertyMetadata(false));


        public ICommand PenaltyCommand
        {
            get { return (ICommand)GetValue(PenaltyCommandProperty); }
            set { SetValue(PenaltyCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PenaltyCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PenaltyCommandProperty =
            DependencyProperty.Register("PenaltyCommand", typeof(ICommand), typeof(PenaltyDisplayControl), new PropertyMetadata(null));

        public PenaltyDisplayControl()
        {
            InitializeComponent();
        }

        private void PenaltyChanged(object sender, EventArgs e)
        {
            var handler = PenaltyCommand;
            if (handler != null)
            {
                var targetPenalty = (Penalty)rating.Value;
                if (targetPenalty != Penalty)
                {
                    handler.Execute(targetPenalty);
                    var expr = GetBindingExpression(PenaltyProperty);
                    Penalty = targetPenalty;
                    BindingOperations.SetBinding(this, PenaltyProperty, expr.ParentBinding);
                }
            }
        }
    }
    
    public class PenaltyToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return PenaltyToInt((Penalty)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return IntToPenalty((int)(double)value);
        }

        private static int PenaltyToInt(Penalty value)
        {
            return (int)value;
        }

        private static Penalty IntToPenalty(int value)
        {
            if (Enum.IsDefined(typeof (Penalty), value))
            {
                return (Penalty)value;
            }

            throw new ArgumentOutOfRangeException("value");
        }

    }

    public class LTRConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = parameter + "";
            if (str == "rotate")
            {
                return (bool)value ? 180.0 : 0.0;
            }
            if (str == "scale")
            {
                return (bool)value ? -1 : 1;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
