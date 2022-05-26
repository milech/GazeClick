using System;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;


namespace GazeClick.Views
{
    /// <summary>
    /// Interaction logic for GazeDot.xaml
    /// </summary>
    public partial class GazeDot : Window
    {
        public const Int32 WM_NCHITTEST = 0x84;
        public const Int32 HTTRANSPARENT = -1;

        public GazeDot()
        {
            InitializeComponent();
        }

        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int GWL_EXSTYLE = -20;

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Get this window's handle
            IntPtr hwnd = new WindowInteropHelper(this).Handle;

            // Change the extended window style to include WS_EX_TRANSPARENT
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            _ = SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
    }
}
