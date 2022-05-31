using System;
using System.ComponentModel;
using System.Windows.Input;
using EyeXFramework.Wpf;
using Tobii.EyeX.Framework;
using GazeClick.Models;
using GazeClick.Commands;


namespace GazeClick.ViewModels
{
    internal class GazeClickViewModel
    {
        private double _timeStamp = 0;
        private readonly WpfEyeXHost _eyeXHost;
        private readonly Log _log;
        private readonly MouseCursor _mouseCursor;

        public GazeClickViewModel() // referenced via Binding in MainWindow.xaml
        {
            App.Current.MainWindow.Closing += new CancelEventHandler(MainWindowClosing);    // TODO: consider refactoring this line using event triggers, not to break the MVVM pattern

            _mouseCursor = MouseCursor.GetInstance();
            _log = Log.GetInstance();
            PunchInCommand = new PunchInCommand(this);
            GazeTimer gazeTimer = GazeTimer.GetInstance(_mouseCursor);

            _eyeXHost = new WpfEyeXHost();
            _eyeXHost.Start();

            gazeTimer.Start();

            EyeXFramework.GazePointDataStream lightlyFilteredGazeDataStream = _eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);

            lightlyFilteredGazeDataStream.Next += (s, e) =>
            {
                try
                {
                    _timeStamp = e.Timestamp;
                    if (_log.IsOn)
                    {
                        _log.Write(string.Format(_log.StandardLogEntry, e.X, e.Y, DateTime.Now, e.Timestamp));
                    }

                    _mouseCursor.CurrentPoint = new MyPoint(e.X, e.Y);

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
            => GazeTimer.GetInstance(_mouseCursor);

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
            _log.Write(string.Format(_log.StandardLogEntry, _mouseCursor.CurrentPoint.X, _mouseCursor.CurrentPoint.Y, DateTime.Now, _timeStamp) + "\tPUNCHED IN");
        }

        public void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Close();
            GazeTimer.Stop();
            _eyeXHost.Dispose();
        }
    }
}
