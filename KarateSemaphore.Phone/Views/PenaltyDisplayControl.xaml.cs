using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using KarateSemaphore.Core;

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

        public static readonly DependencyProperty PenaltyProperty = DependencyProperty.Register("Penalty", typeof(Penalty), typeof(PenaltyDisplayControl), new PropertyMetadata(Penalty.None, OnPenaltyChanged));

        private static void OnPenaltyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!Enum.IsDefined(typeof (Penalty), e.NewValue)) return;
            var value = (int)e.NewValue;
            var instance = AssureThis(d);
            for (var i = 0; i < instance._controls.Count; i++)
            {
                var ctrl = instance._controls[i];
                ctrl.IsEnabled = i < value;
            }
        }

        private static PenaltyDisplayControl AssureThis(DependencyObject d)
        {
            var instance = d as PenaltyDisplayControl;
            if(instance == null)
            {
                throw new InvalidOperationException("Expected a source of same type");
            }
            return instance;
        }

        private readonly List<Control> _controls;

        public PenaltyDisplayControl()
        {
            InitializeComponent();
            _controls = new List<Control> { ctrl1, ctrl2, ctrl3, ctrl4 };
            _controls.ForEach(x => x.IsEnabled = false);
        }
    }
}
