using System;
using System.Windows;
using System.Threading;
using System.ComponentModel;
using System.Runtime.InteropServices;
using GazeClick.ViewModels;

namespace GazeClick.Models
{
    internal class MouseCursor : INotifyPropertyChanged
    {
        private static MouseCursor _instance;
        private const int _DeltaThr = 400;
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

        public MyPoint CurrentPoint
        {
            get; set;
        }

        public MyPoint PreviousPoint
        {
            get; set;
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

        public static int DeltaThr => _DeltaThr;

        public void SetCursorPosition(int x, int y)
        {
            _ = SetCursorPos(x, y);
        }

        public void SetCursorPosition()
        {
            _ = SetCursorPos((int)CurrentPoint.X, (int)CurrentPoint.Y);
        }

        public int GetDeltaX()
        {
            return Math.Abs(CurrentPoint.X - PreviousPoint.X);
        }

        public int GetDeltaY()
        {
            return Math.Abs(CurrentPoint.Y - PreviousPoint.Y);
        }

        public void UpdatePreviousPoint()
        {
            if (CurrentPoint != null)
            {
                PreviousPoint = CurrentPoint.Clone();
            }
        }

        public void EmulateClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, Convert.ToUInt32(CurrentPoint.X), Convert.ToUInt32(CurrentPoint.Y), 0, 0);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_LEFTUP, Convert.ToUInt32(CurrentPoint.X), Convert.ToUInt32(CurrentPoint.Y), 0, 0);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
