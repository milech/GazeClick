// -----------------------------------------------------------
// GazeClick - App for controlling mouse cursor and emulating
// mouse clicks with gaze tracked using Tobii eye-trackers
// - class representing a mouse cursor
// (C) 2022 Michal Lech, Gdynia, Poland
// Released under GNU General Public License v3.0 (GPL-3.0)
// email: mlech.ksm@gmail.com
//-----------------------------------------------------------

using System;
using System.Threading;
using System.Runtime.InteropServices;

namespace GazeClick.Models
{
    internal class MouseCursor
    {
        private static MouseCursor _instance;
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
            CurrentPoint = new MyPoint(0, 0);
            PreviousPoint = new MyPoint(0, 0);
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

        public MyPoint CurrentPoint { get; set; }

        public MyPoint PreviousPoint { get; set; }

        /// <summary>
        /// Sets mouse cursor position on the screen based on given (x, y), via external User32.dll
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetCursorPosition(int x, int y)
        {
            _ = SetCursorPos(x, y);
        }

        /// <summary>
        /// Sets mouse cursor position on the screen according to the CurrentPoint, via external User32.dll
        /// </summary>
        public void SetCursorPosition()
        {
            _ = SetCursorPos(CurrentPoint.X, CurrentPoint.Y);
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
    }
}
