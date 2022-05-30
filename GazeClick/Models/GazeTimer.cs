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
        private Log _log;
        private static readonly object _lock = new object();

        private GazeTimer(int time, int minTime, int maxTime)
        {
            Time = time;
            MinTime = minTime;
            MaxTime = maxTime;
            Tick += GazeTimer_Tick;
        }

        public static GazeTimer GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GazeTimer(3000, 500, 5000);
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

        public void SetLog(Log log)
        {
            _log = log;
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
                _log.Write("dummy - tick");
            //    currentPoint.X = (int) gazeDot.Left;
            //    currentPoint.Y = (int) gazeDot.Top;
            //    int xDiff = Math.Abs(prevPoint.X - currentPoint.X);
            //    int yDiff = Math.Abs(prevPoint.Y - currentPoint.Y);

            //    if (currentPoint.X > 0 && currentPoint.Y > 0 &&
            //        xDiff != 0 && yDiff != 0 && xDiff < 400 && yDiff < 400 &&
            //        emulateClicksCheckbox.IsChecked == true)
            //    {
            //        //%mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Convert.ToUInt32(p.X), Convert.ToUInt32(p.Y), 0, 0);
            //        mouse_event(MOUSEEVENTF_LEFTDOWN, Convert.ToUInt32(currentPoint.X), Convert.ToUInt32(currentPoint.Y), 0, 0);
            //        Thread.Sleep(100);
            //        mouse_event(MOUSEEVENTF_LEFTUP, Convert.ToUInt32(currentPoint.X), Convert.ToUInt32(currentPoint.Y), 0, 0);
            //    }

            //    prevPoint.X = currentPoint.X;
            //    prevPoint.Y = currentPoint.Y;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
