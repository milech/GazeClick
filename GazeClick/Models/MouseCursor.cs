using System.ComponentModel;
using System.Runtime.InteropServices;

namespace GazeClick.Models
{
    internal class MouseCursor : INotifyPropertyChanged
    {
        private static MouseCursor _instance;
        private bool _isMoving = false;
        private bool _isClicking = false;
        private static readonly object _lock = new object();

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        //private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        //private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private MouseCursor()
        {
        }

        public static MouseCursor GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new MouseCursor();
                    }
                }
            }
            return _instance;
        }

        public bool IsMoving
        {
            get
            {
                return _isMoving;
            }
            set
            {
                _isMoving = value;
                OnPropertyChanged("IsMoving");
            }
        }

        public bool IsClicking
        {
            get
            {
                return _isClicking;
            }
            set
            {
                _isClicking = value;
                OnPropertyChanged("IsClicking");
            }
        }

        public void SetCursorPosition(int x, int y)
        {
            _ = SetCursorPos(x, y);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
