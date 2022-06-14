// -----------------------------------------------------------
// GazeClick - App for controlling mouse cursor and emulating
// mouse clicks with gaze tracked using Tobii eye-trackers
// - timer for controlling mouse clicking emulation
// (C) 2022 Michal Lech, Gdynia, Poland
// Released under GNU General Public License v3.0 (GPL-3.0)
// email: mlech.ksm@gmail.com
//-----------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Threading;

namespace GazeClick.Models
{
    internal class GazeTimer : DispatcherTimer, INotifyPropertyChanged
    {
        private static GazeTimer _instance;
        private int _time;
        private readonly MouseCursor _mouseCursor;
        private readonly Log _log;
        private static readonly object _lock = new object();

        private GazeTimer(int time, int minTime, int maxTime, MouseCursor mouseCursor, Log log)
        {
            Time = time;
            MinTime = minTime;
            MaxTime = maxTime;
            _mouseCursor = mouseCursor;
            _log = log;
            Tick += GazeTimer_Tick;
        }

        public static GazeTimer GetInstance(MouseCursor mouseCursor, Log log)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GazeTimer(3000, 500, 5000, mouseCursor, log);
                    }
                }
            }
            return _instance;
        }

        public int Time
        {
            get 
            {
                return _time;
            }
            set
            {
                try
                {
                    _time = value;
                    Interval = new TimeSpan(0, 0, 0, 0, _time);
                    OnPropertyChanged("Time");
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        public int MinTime
        {
            get; private set;
        }

        public int MaxTime
        {
            get; private set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GazeTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // emulate mouse click if cursor stays in the same place (defined by DeltaThr)
                // for longer than this.Time
                if (_mouseCursor.IsClicking && _mouseCursor.IsMoving
                    &&_mouseCursor.CurrentPoint.X > 0 && _mouseCursor.CurrentPoint.Y > 0
                    && _mouseCursor.GetDeltaX() != 0 && _mouseCursor.GetDeltaY() != 0
                    && _mouseCursor.GetDeltaX() < MouseCursor.DeltaThr
                    && _mouseCursor.GetDeltaY() < MouseCursor.DeltaThr)
                {
                    _mouseCursor.EmulateClick();
                    _log.Write(string.Concat(string.Format(
                        _log.StandardLogEntry,
                        _mouseCursor.CurrentPoint.X,
                        _mouseCursor.CurrentPoint.Y,
                        DateTime.Now,
                        -1), "\tdeltaX = ", _mouseCursor.GetDeltaX(), "\tdeltaY = ", _mouseCursor.GetDeltaY(), "\tCLICKED"));
                }

                _mouseCursor.UpdatePreviousPoint();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
