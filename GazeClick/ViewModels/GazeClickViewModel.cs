// -----------------------------------------------------------
// GazeClick - App for controlling mouse cursor and emulating
// mouse clicks with gaze tracked using Tobii eye-trackers
// - ViewModel following the MVVM pattern
// (C) 2022 Michal Lech, Gdynia, Poland
// Released under GNU General Public License v3.0 (GPL-3.0)
// email: mlech.ksm@gmail.com
//-----------------------------------------------------------

using System;
using System.ComponentModel;
using System.Windows.Input;
using EyeXFramework.Wpf;
using Tobii.EyeX.Framework;
using GazeClick.Models;
using GazeClick.Commands;
using MyKalmanFilterDLL;


namespace GazeClick.ViewModels
{
    internal class GazeClickViewModel
    {
        private double _timeStamp = 0;
        private readonly WpfEyeXHost _eyeXHost;
        private readonly Log _log;
        private readonly MouseCursor _mouseCursor;
        private readonly MyKalmanFilter _kalmanFilter;

        public GazeClickViewModel() // referenced via Binding in MainWindow.xaml
        {
            App.Current.MainWindow.Closing += new CancelEventHandler(MainWindowClosing);    // TODO: consider refactoring this line using event triggers, not to break (so roughly) the MVVM pattern

            _mouseCursor = MouseCursor.GetInstance();
            _log = Log.GetInstance();
            PunchInCommand = new PunchInCommand(this);
            GazeTimer gazeTimer = GazeTimer.GetInstance(_mouseCursor, _log);

            _kalmanFilter = new MyKalmanFilter();

            _eyeXHost = new WpfEyeXHost();
            _eyeXHost.Start();

            gazeTimer.Start();

            EyeXFramework.GazePointDataStream lightlyFilteredGazeDataStream = _eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);

            lightlyFilteredGazeDataStream.Next += (s, e) =>
            {
                try
                {
                    MyPoint point;
                    if (_mouseCursor.IsSmoothening)
                    {
                        _kalmanFilter.filter(e.X, e.Y, 0, 0);
                        point = new MyPoint(_kalmanFilter.getX(), _kalmanFilter.getY());
                    }
                    else
                    {
                        point = new MyPoint(e.X, e.Y);
                    }

                    if (_log.IsOn)
                    {
                        _log.Write(string.Concat(string.Format(_log.StandardLogEntry, point.X, point.Y, DateTime.Now, e.Timestamp), "\tdeltaX = ", _mouseCursor.GetDeltaX(), "\tdeltaY = ", _mouseCursor.GetDeltaY()));
                    }
                    _timeStamp = e.Timestamp;

                    _mouseCursor.CurrentPoint = point;

                    if (_mouseCursor.IsMoving)
                    {
                        _mouseCursor.SetCursorPosition();   // moves mouse cursor on the screen
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    if (_log.IsOn)
                    {
                        _log.Write(ex.Message);
                    }
                }
            };
        }

        public GazeTimer GazeTimer  // Referenced via binding in MainWindow
            => GazeTimer.GetInstance(_mouseCursor, _log);

        public Log Log  // Referenced via binding in MainWindow
            => Log.GetInstance();

        public MouseCursor MouseCursor  // Referenced via binding in MainWindow
            => MouseCursor.GetInstance();

        public bool CanPunchIn
            => _log == null ? false : Log.IsOn;

        public ICommand PunchInCommand  // Referenced via binding in MainWindow
        {
            get;
            private set;
        }

        public void PunchInRegister()
        {
            _log.Write(string.Concat(string.Format(_log.StandardLogEntry, _mouseCursor.CurrentPoint.X, _mouseCursor.CurrentPoint.Y, DateTime.Now, _timeStamp), "\tdeltaX = ", _mouseCursor.GetDeltaX(), "\tdeltaY = ", _mouseCursor.GetDeltaY(), "\tPUNCHED IN"));
        }

        public void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Close();
            GazeTimer.Stop();
            _eyeXHost.Dispose();
        }
    }
}
