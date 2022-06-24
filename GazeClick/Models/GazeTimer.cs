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
using GazeClick.ViewModels;

namespace GazeClick.Models
{
    internal class GazeTimer : DispatcherTimer, INotifyPropertyChanged
    {
        private static GazeTimer _instance;
        private readonly GazeClickViewModel _viewModel;
        private readonly MouseCursor _mouseCursor;
        private readonly Log _log;
        private readonly MyConfig _config;
        private int _time;
        private static readonly object _lock = new object();

        private GazeTimer(GazeClickViewModel viewModel, MouseCursor mouseCursor, Log log, MyConfig config)
        {
            _viewModel = viewModel;
            _mouseCursor = mouseCursor;
            _log = log;
            _config = config;
            ClickTime = config.ClickTime;
            Tick += GazeTimer_Tick;
        }

        public static GazeTimer GetInstance(GazeClickViewModel viewModel, MouseCursor mouseCursor, Log log, MyConfig config)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GazeTimer(viewModel, mouseCursor, log, config);
                    }
                }
            }
            return _instance;
        }

        public int ClickTime
        {
            get => _time;
            set
            {
                try
                {
                    _time = value;
                    Interval = new TimeSpan(0, 0, 0, 0, _time);
                    _config.ClickTime = _time;
                    OnPropertyChanged("ClickTime");
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
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
                if (_config.IsCursorClicking && _config.IsCursorMoving
                    &&_mouseCursor.CurrentPoint.X > 0 && _mouseCursor.CurrentPoint.Y > 0
                    && _mouseCursor.GetDeltaX() != 0 && _mouseCursor.GetDeltaY() != 0
                    && _mouseCursor.GetDeltaX() < _config.DeltaThr
                    && _mouseCursor.GetDeltaY() < _config.DeltaThr)
                {
                    _mouseCursor.EmulateClick();
                    _log.Write(string.Concat(string.Format(
                        _log.StandardLogEntry,
                        _mouseCursor.CurrentPoint.X,
                        _mouseCursor.CurrentPoint.Y,
                        DateTime.Now,
                        _viewModel.CurrentTimeStamp), "\tdeltaX = ", _mouseCursor.GetDeltaX(), "\tdeltaY = ", _mouseCursor.GetDeltaY(), "\tCLICKED"));
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
