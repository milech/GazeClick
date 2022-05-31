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
        private static readonly object _lock = new object();

        private GazeTimer(int time, int minTime, int maxTime, MouseCursor mouseCursor)
        {
            Time = time;
            MinTime = minTime;
            MaxTime = maxTime;
            _mouseCursor = mouseCursor;
            Tick += GazeTimer_Tick;
        }

        public static GazeTimer GetInstance(MouseCursor mouseCursor)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GazeTimer(3000, 500, 5000, mouseCursor);
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
                if (_mouseCursor.IsClicking
                    &&_mouseCursor.CurrentPoint.X > 0 && _mouseCursor.CurrentPoint.Y > 0
                    && _mouseCursor.GetDeltaX() != 0 && _mouseCursor.GetDeltaY() != 0
                    && _mouseCursor.GetDeltaX() < MouseCursor.DeltaThr
                    && _mouseCursor.GetDeltaY() < MouseCursor.DeltaThr)
                {
                    _mouseCursor.EmulateClick();
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
