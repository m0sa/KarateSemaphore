using System;
using System.Windows;
using System.Windows.Interop;
using KarateSemaphore.Core;

namespace KarateSemaphore
{
    /// <summary>
    /// Interaction logic for DisplayWindow.xaml
    /// </summary>
    public partial class DisplayWindow
    {
        private readonly WindowStyle _normalWindowStyle;

        public DisplayWindow(ISemaphore semaphore)
        {
            InitializeComponent();
            DataContext = semaphore;
            _normalWindowStyle = WindowStyle;
            var hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).EnsureHandle());
            if (hwndSource != null)
            {
                hwndSource.AddHook(FullScreenHook);
            }
        }


        private void ToNormal()
        {
            Topmost = false;
            WindowStyle = _normalWindowStyle;
            WindowState = WindowState.Normal;
        }

        private IntPtr FullScreenHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int wmSyscommand = 0x112;
            const int scMaximize = 0xF030;
            const int scClose = 0xF060;
            const int scRestore = 0xF120;

            if (msg == wmSyscommand)
            {
                var sysCommand = wParam.ToInt32();
                switch (sysCommand)
                {
                    case scMaximize:
                        Topmost = true;
                        WindowStyle = WindowStyle.None;
                        break;
                    case scClose:
                        ToNormal();
                        handled = true;
                        break;
                    case scRestore:
                        ToNormal();
                        break;
                }
            }
            return IntPtr.Zero;
        }
    }
}
