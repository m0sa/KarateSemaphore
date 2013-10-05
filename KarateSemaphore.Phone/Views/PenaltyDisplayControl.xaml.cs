using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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
}
