using System;
using System.Windows;
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
        //private GazeDot gazeDot;
        private double _timeStamp = 0;
        private readonly WpfEyeXHost _eyeXHost;
        private readonly Log log;
        private Point _currentPoint;
        private Point _prevPoint;

        public GazeClickViewModel() // referenced via Binding in MainWindow.xaml
        {
            App.Current.MainWindow.Closing += new CancelEventHandler(MainWindowClosing);    // TODO: consider refactoring this line using event triggers, not to break the MVVM pattern

            MouseCursor mouseCursor = MouseCursor.GetInstance();
            log = Log.GetInstance();
            PunchInCommand = new PunchInCommand(this);
            GazeTimer gazeTimer = GazeTimer.GetInstance();
            gazeTimer.SetLog(log);

            //Console.SetOut(log.GetStreamWriter());

            _eyeXHost = new WpfEyeXHost();
            _eyeXHost.Start();

            _prevPoint = new Point();
            _currentPoint = new Point();

            //gazeDot = new GazeDot();

            gazeTimer.Start();

            EyeXFramework.GazePointDataStream lightlyFilteredGazeDataStream = _eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);

            lightlyFilteredGazeDataStream.Next += (s, e) =>
            {
                try
                {
                    _timeStamp = e.Timestamp;
                    _currentPoint.X = e.X;
                    _currentPoint.Y = e.Y;
                    if (log.IsOn)
                    {
                        log.Write(string.Format(log.StandardLogEntry, e.X, e.Y, DateTime.Now, e.Timestamp));
                    }

                    //SetDotPosition(e);

                    if (mouseCursor.IsMoving)
                    {
                        mouseCursor.SetCursorPosition((int)e.X, (int)e.Y);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    if (log.IsOn)
                    {
                        log.Write(ex.Message);
                    }
                }
            };
        }

        public GazeTimer GazeTimer  // Referenced via binding in MainWindow
            => GazeTimer.GetInstance();

        public Log Log  // Referenced via binding in MainWindow
            => Log.GetInstance();

        public MouseCursor MouseCursor  // Referenced via binding in MainWindow
            => MouseCursor.GetInstance();

        public bool CanPunchIn
            => log == null ? false : Log.IsOn;

        public ICommand PunchInCommand  // Referenced via binding in MainWindow
        {
            get;
            private set;
        }

        public void PunchInRegister()
        {
            log.Write(string.Format(log.StandardLogEntry, _currentPoint.X, _currentPoint.Y, DateTime.Now, _timeStamp) + "\tPUNCHED IN");
        }

        public void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Close();
            //gazeDot.Close();
            GazeTimer.Stop();
            _eyeXHost.Dispose();
        }
    }
}
