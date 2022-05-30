using System;
using System.Runtime.InteropServices;
using EyeXFramework.Wpf;
using Tobii.EyeX.Framework;
using GazeClick.Models;
using System.ComponentModel;

namespace GazeClick.ViewModels
{
    internal class GazeClickViewModel
    {
        //private GazeDot gazeDot;
        private int clicksCounter = 0;
        private double _timeStamp = 0;
        private readonly WpfEyeXHost _eyeXHost;
        private MyPoint _currentPoint;
        private MyPoint _prevPoint;

        public GazeClickViewModel() // referenced via Binding in MainWindow.xaml
        {
            App.Current.MainWindow.Closing += new CancelEventHandler(MainWindowClosing);    // TODO: consider refactoring using event triggers not to break the MVVM pattern

            MouseCursor mouseCursor = MouseCursor.GetInstance();
            Log log = Log.GetInstance();
            GazeTimer gazeTimer = GazeTimer.GetInstance();
            gazeTimer.SetLog(log);

            //Console.SetOut(log.GetStreamWriter());

            _eyeXHost = new WpfEyeXHost();
            _eyeXHost.Start();

            _currentPoint = new MyPoint();
            _prevPoint = new MyPoint();

            //gazeDot = new GazeDot();

            gazeTimer.Start();

            EyeXFramework.GazePointDataStream lightlyFilteredGazeDataStream = _eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered);

            lightlyFilteredGazeDataStream.Next += (s, e) =>
            {
                try
                {
                    if (log.IsOn)
                    {
                        log.Write(string.Format("{0:0.0}\t{1:0.0}\t{2:MM/dd/yy H:mm:ss fffffff}\t{3:0} ", e.X, e.Y, DateTime.Now, e.Timestamp));
                        _timeStamp = e.Timestamp;
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

            //registerButton.Click += (s, e) =>
            //{
            //    try
            //    {
            //        clicksCounter++;
            //        Console.WriteLine(clicksCounter.ToString() + ": ----------- Gaze point at ({0:0.0}, {1:0.0}) t:{2:MM/dd/yy H:mm:ss fffffff} @{3:0} -----------", gazeDot.Left, gazeDot.Top, DateTime.Now, timeStamp);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //};
        }

        public GazeTimer GazeTimer  // Referenced via binding in MainWindow
            => GazeTimer.GetInstance();

        public Log Log  // Referenced via binding in MainWindow
            => Log.GetInstance();

        public MouseCursor MouseCursor  // Referenced via binding in MainWindow
            => MouseCursor.GetInstance();

        public void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Close();
            //gazeDot.Close();
            GazeTimer.Stop();
            _eyeXHost.Dispose();
            //App.Current.Shutdown();
        }
    }
}
